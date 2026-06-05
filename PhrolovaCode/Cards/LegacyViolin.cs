
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class LegacyViolin : PhrolovaCard
    {
        public LegacyViolin() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            IsUpgraded ? new[] { CardKeyword.Retain, CardKeyword.Exhaust } : new[] { CardKeyword.Exhaust };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var handPile = PileType.Hand.GetPile(Owner);
            if (handPile == null) return;

            var cardsInHand = handPile.Cards.ToList(); // 快照当前手牌
            int count = cardsInHand.Count;
            if (count == 0) return;

            // 消耗所有手牌（除了自身？官方类似卡牌一般不包括自身，但这里是“消耗你的所有手牌”）
            foreach (var card in cardsInHand)
                await CardCmd.Exhaust(choiceContext, card);

            // 抽等量牌
            await CardPileCmd.Draw(choiceContext, count, Owner);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}