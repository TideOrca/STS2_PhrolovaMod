
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class ForkPath : PhrolovaCard
    {
        public ForkPath() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new PowerVar<WeakPower>(3m),
            new DynamicVar("Resonance", 4m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this, false);
            int resonanceStacks = (int)DynamicVars["Resonance"].BaseValue;
            await PowerCmd.Apply<ResonancePower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, resonanceStacks, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Resonance"].UpgradeValueBy(2m);
        }
    }
}