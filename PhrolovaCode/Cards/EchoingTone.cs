
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class EchoingTone : PhrolovaCard
    {
        public EchoingTone() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(3m, ValueProp.Move),
            new DynamicVar("ResonanceBonus", 3m) // 每层余响额外伤害基数
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var resonance = Owner.Creature.Powers.OfType<ResonancePower>().FirstOrDefault();
            int resonanceStacks = resonance != null ? resonance.Amount : 0;
            decimal totalDamage = DynamicVars.Damage.BaseValue + resonanceStacks * DynamicVars["ResonanceBonus"].BaseValue;

            await DamageCmd.Attack(totalDamage)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ResonanceBonus"].UpgradeValueBy(1m); // 3 → 4
        }
    }
}