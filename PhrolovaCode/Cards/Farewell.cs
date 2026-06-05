
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Farewell : PhrolovaCard
    {
        public Farewell() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(8m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            // 基础效果：对所有敌人造成伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(combatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 重世效果：再次对所有敌人造成等量伤害
            if (await TryConsumeRebirth())
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .TargetingAllOpponents(combatState)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m); // 8 → 12
        }
    }
}