using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Rope : PhrolovaCard
    {
        public Rope() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        // 动态变量：基础施加层数 50
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("Stacks", 50m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["Stacks"].BaseValue;
            await PowerCmd.Apply<RopePower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, stacks, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Stacks"].UpgradeValueBy(25m); // 50 → 75
        }
    }
}