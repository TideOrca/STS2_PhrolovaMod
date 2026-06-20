
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class ChaosFrequency : PhrolovaCard
    {
        public ChaosFrequency() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new EnergyVar(1),  // 下回合获得的能量
            new PowerVar<WeakPower>(1m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 下回合获得能量（官方能力）
            await PowerCmd.Apply<EnergyNextTurnPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, DynamicVars.Energy.BaseValue, Owner.Creature, this, false);

            // 给自己1层虚弱
            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Energy.UpgradeValueBy(1m); // 1 → 2
        }
    }
}