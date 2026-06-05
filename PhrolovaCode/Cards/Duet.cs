
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Duet : PhrolovaCard
    {
        public Duet() : base(1, CardType.Attack, CardRarity.Common, TargetType.None) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(5m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            decimal damage = DynamicVars.Damage.BaseValue;

            // 弗洛洛攻击全体
            await DamageCmd.Attack(damage)
                .FromCard(this)
                .TargetingAllOpponents(combatState)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 赫卡忒攻击全体
            var hecate = Owner.Creature.Pets.FirstOrDefault(p => p.Monster is Hecate);
            if (hecate != null && hecate.IsAlive)
            {
                var enemies = combatState.GetOpponentsOf(Owner.Creature);
                await CreatureCmd.Damage(choiceContext, enemies, damage, ValueProp.Unpowered, hecate);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m); // 5 → 7
        }
    }
}