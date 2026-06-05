
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Prelude : PhrolovaCard
    {
        public Prelude() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new CardsVar(2)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 基础效果
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

            // 重世效果
            if (await TryConsumeRebirth())
            {
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1m);
        }
    }
}