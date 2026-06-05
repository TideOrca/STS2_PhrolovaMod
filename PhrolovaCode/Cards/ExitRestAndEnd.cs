
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class ExitRestAndEnd : PhrolovaCard
    {
        public ExitRestAndEnd() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(8m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            var resonancePowers = Owner.Creature.Powers.OfType<ResonancePower>().ToList();
            int totalResonance = resonancePowers.Sum(p => p.Amount);
            if (totalResonance <= 0) return;

            // 对所有敌人造成 totalResonance 次伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(totalResonance)
                .FromCard(this)
                .TargetingAllOpponents(combatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 清空所有余响
            foreach (var p in resonancePowers)
                await PowerCmd.Remove(p);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m); // 8 → 11
        }
    }
}