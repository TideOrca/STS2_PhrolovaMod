
using Phrolova.PhrolovaCode.Powers;
using Phrolova.PhrolovaCode.Utils;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class DispellingDelusions : PhrolovaCard
    {
        public DispellingDelusions() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(21m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            // 造成伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 将所有乐声变为彩乐
            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
            {
                await NoteActions.ConvertAllToColorful(tuning);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(9m);
        }
    }
}