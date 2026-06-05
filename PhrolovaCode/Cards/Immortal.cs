
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Immortal : PhrolovaCard
    {
        protected override bool HasEnergyCostX => true;

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("PlatingPerX", 2m),
            new DynamicVar("DexterityPerX", 1m)
        };

        public Immortal() : base(-1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int x = ResolveEnergyXValue();
            if (x <= 0) return;

            int platingStacks = (int)DynamicVars["PlatingPerX"].BaseValue * x;
            int dexStacks = (int)DynamicVars["DexterityPerX"].BaseValue * x;

            if (platingStacks > 0)
                await PowerCmd.Apply<PlatingPower>(Owner.Creature, platingStacks, Owner.Creature, this);

            if (dexStacks > 0)
                await PowerCmd.Apply<DexterityPower>(Owner.Creature, dexStacks, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["PlatingPerX"].UpgradeValueBy(1m); // 2X → 3X
        }
    }
}