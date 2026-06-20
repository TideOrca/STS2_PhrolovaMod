
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class FleetingDream : PhrolovaCard
    {
        public FleetingDream() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(6m, ValueProp.Move)
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromKeyword(PhrolovaCode.Keywords.Rebirth)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 1. 先判断重世：当前已有重世层数才触发蓝乐，免得自己给的重世被自己消耗
            if (await TryConsumeRebirth())
            {
                var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
                if (tuning != null)
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Blue, 1, choiceContext);
            }

            // 2. 基础效果：防御
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            // 3. 获得一层重世（无论之前是否有，都会加上）
            await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}