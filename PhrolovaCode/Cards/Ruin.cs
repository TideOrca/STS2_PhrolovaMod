
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Ruin : PhrolovaCard
    {
        protected override bool HasEnergyCostX => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(10m, ValueProp.Move)
        };

        public Ruin() : base(-1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int x = ResolveEnergyXValue();

            // 攻击 X 次（官方 Eradicate 写法：WithHitCount）
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(x)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 获得 X 层余响
            if (x > 0)
                await PowerCmd.Apply<ResonancePower>(Owner.Creature, x, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m); // 10 → 13
        }
    }
}