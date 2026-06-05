
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class LostAndFound : PhrolovaCard
    {
        public LostAndFound() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new EnergyVar(2)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (await TryConsumeRebirth())
            {
                await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1m);
        }
    }
}