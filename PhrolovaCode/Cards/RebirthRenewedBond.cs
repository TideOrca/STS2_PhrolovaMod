using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class RebirthRenewedBond : PhrolovaCard
    {
        public RebirthRenewedBond() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("Stacks", 7m)   // 基础7层，升级后变为10
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["Stacks"].BaseValue;
            await PowerCmd.Apply<RebirthRenewedBondPower>(Owner.Creature, stacks, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Stacks"].UpgradeValueBy(3m); // 7 → 10
        }
    }
}