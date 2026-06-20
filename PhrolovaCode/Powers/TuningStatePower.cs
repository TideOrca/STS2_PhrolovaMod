
using Phrolova.PhrolovaCode.Cards;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class TuningStatePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;

        private int _cardsPlayedThisCycle;
        private readonly List<NoteType> _noteOrder = new();
        private bool _hasGivenFinalMovement;

        public enum NoteType { Red, Blue, Colorful }

        protected override IEnumerable<IHoverTip> ExtraHoverTips =>
            HoverTipFactory.FromCardWithCardHoverTips<FinalMovement>(false);

        // 静态事件：保留给音效等不影响游戏状态的功能
        public static event Action<Creature, NoteType> NoteGained;

        // 公共属性：允许辅助类访问顺序列表
        internal List<NoteType> NoteOrder => _noteOrder;

       
        public async Task ForceAddNote(NoteType type, int count = 1, PlayerChoiceContext? context = null)
        {
            for (int i = 0; i < count; i++)
            {
                await AddNoteInternal(type, context);
            }
            await CheckFullNotes();
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner?.Creature != Owner) return;

            NoteType? noteType = cardPlay.Card.Type switch
            {
                CardType.Attack => NoteType.Red,
                CardType.Skill => NoteType.Blue,
                CardType.Power => NoteType.Colorful,
                _ => null
            };
            if (noteType == null) return;

            _cardsPlayedThisCycle++;
            if (_cardsPlayedThisCycle < 3) return;
            _cardsPlayedThisCycle = 0;

            await AddNoteInternal(noteType.Value, context);
            Flash();
            await CheckFullNotes();
        }

        private async Task AddNoteInternal(NoteType type, PlayerChoiceContext? context = null)
        {
            int total = GetCurrentTotal();
            var ctx = context ?? new ThrowingPlayerChoiceContext();

            // 乐声数量达到上限时，替换最早的非彩乐
            if (total >= 6)
            {
                NoteType? firstNonColorful = null;
                int? indexToRemove = null;
                for (int i = 0; i < _noteOrder.Count; i++)
                {
                    if (_noteOrder[i] != NoteType.Colorful)
                    {
                        firstNonColorful = _noteOrder[i];
                        indexToRemove = i;
                        break;
                    }
                }

                if (firstNonColorful == null) return;
                _noteOrder.RemoveAt(indexToRemove.Value);
                if (firstNonColorful.Value == NoteType.Red)
                {
                    var toModify = Owner.Powers.OfType<RedNotePower>().FirstOrDefault();
                    if (toModify != null && toModify.Amount > 0)
                        await PowerCmd.ModifyAmount(ctx, toModify, -1, Owner, null, false);
                }
                else if (firstNonColorful.Value == NoteType.Blue)
                {
                    var toModify = Owner.Powers.OfType<BlueNotePower>().FirstOrDefault();
                    if (toModify != null && toModify.Amount > 0)
                        await PowerCmd.ModifyAmount(ctx, toModify, -1, Owner, null, false);
                }
            }

            _noteOrder.Add(type);
            switch (type)
            {
                case NoteType.Red: await PowerCmd.Apply<RedNotePower>(ctx, new[] { Owner }, 1, Owner, null, false); break;
                case NoteType.Blue: await PowerCmd.Apply<BlueNotePower>(ctx, new[] { Owner }, 1, Owner, null, false); break;
                case NoteType.Colorful: await PowerCmd.Apply<ColorfulNotePower>(ctx, new[] { Owner }, 1, Owner, null, false); break;
            }

            // 火炬抽牌效果
            var torchPowers = Owner.Powers.OfType<TorchPower>();
            int totalDraws = torchPowers.Sum(p => p.Amount);
            if (totalDraws > 0 && Owner.Player != null)
            {
                await CardPileCmd.Draw(ctx, totalDraws, Owner.Player);
            }

            // 触发事件供非关键功能（如音效）使用
            NoteGained?.Invoke(Owner, type);
        }

        internal int GetCurrentTotal()
        {
            if (Owner == null) return 0;
            int red = (int)Owner.Powers.OfType<RedNotePower>().Sum(p => p.Amount);
            int blue = (int)Owner.Powers.OfType<BlueNotePower>().Sum(p => p.Amount);
            int colorful = (int)Owner.Powers.OfType<ColorfulNotePower>().Sum(p => p.Amount);
            return red + blue + colorful;
        }

        private async Task CheckFullNotes()
        {
            if (_hasGivenFinalMovement) return;
            if (GetCurrentTotal() < 6 || Owner?.Player == null) return;

            // 检查牌组中是否已有谱曲终末（手牌/抽牌堆/弃牌堆）
            bool hasFinalMovement = false;
            foreach (var pileType in new[] { PileType.Hand, PileType.Draw, PileType.Discard })
            {
                var pile = pileType.GetPile(Owner.Player);
                if (pile != null && pile.Cards.Any(c => c is FinalMovement))
                {
                    hasFinalMovement = true;
                    break;
                }
            }
            if (hasFinalMovement)
            {
                _hasGivenFinalMovement = true;
                return;
            }

            await CardPileCmd.AddToCombatAndPreview<FinalMovement>(
                new[] { Owner },
                PileType.Hand,
                1,
                Owner?.Player,
                CardPilePosition.Top
            );

            _hasGivenFinalMovement = true;
        }
    }
}