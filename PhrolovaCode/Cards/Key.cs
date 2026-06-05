
using MegaCrit.Sts2.Core.CardSelection;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Key : PhrolovaCard
    {
        public Key() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromCard<SecretOfUnderworld>(IsUpgraded)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            var drawPile = PileType.Draw.GetPile(Owner);
            if (drawPile == null || drawPile.Cards.Count == 0) return;

            // 从抽牌堆中选一张牌
            var cards = drawPile.Cards.ToList();
            var selected = (await CardSelectCmd.FromSimpleGrid(
                choiceContext,
                cards,
                Owner,
                new CardSelectorPrefs(CardSelectorPrefs.TransformSelectionPrompt, 1)
            )).FirstOrDefault();

            if (selected == null) return;

            // 变成奥秘
            CardPileAddResult? result = await CardCmd.TransformTo<SecretOfUnderworld>(selected);
            if (IsUpgraded && result.HasValue)
            {
                CardCmd.Upgrade(result.Value.cardAdded);
            }
        }

        protected override void OnUpgrade() { }
    }
}