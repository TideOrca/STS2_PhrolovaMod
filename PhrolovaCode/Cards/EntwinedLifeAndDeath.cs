using MegaCrit.Sts2.Core.Commands;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class EntwinedLifeAndDeath : PhrolovaCard
    {
        public EntwinedLifeAndDeath() : base(4, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(4m, ValueProp.Move),
            new DynamicVar("Hits", 4m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            int hits = (int)DynamicVars["Hits"].BaseValue;

            // 基础效果：对全体敌人造成 hits 次伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(hits)
                .FromCard(this)
                .TargetingAllOpponents(combatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 重世效果：再造成 hits 次全体伤害
            if (await TryConsumeRebirth())
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .WithHitCount(hits)
                    .FromCard(this)
                    .TargetingAllOpponents(combatState)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}