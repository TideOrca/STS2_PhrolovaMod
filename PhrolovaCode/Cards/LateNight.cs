
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class LateNight : PhrolovaCard
    {
        public LateNight() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("VigorBase", 5m),
            new DynamicVar("RebirthVigor", 5m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int vigorBase = (int)DynamicVars["VigorBase"].BaseValue;
            await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, vigorBase, Owner.Creature, this, false);

            if (await TryConsumeRebirth())
            {
                int extraVigor = (int)DynamicVars["RebirthVigor"].BaseValue;
                await PowerCmd.Apply<VigorPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, extraVigor, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["VigorBase"].UpgradeValueBy(2m);
            DynamicVars["RebirthVigor"].UpgradeValueBy(3m);
        }
    }
}