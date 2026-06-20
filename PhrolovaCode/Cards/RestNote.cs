
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class RestNote : PhrolovaCard
    {
        public RestNote() : base(0, CardType.Attack, CardRarity.Rare, TargetType.None) { }

        protected override bool IsPlayable
        {
            get
            {
                if (Owner == null) return false;
                int colorful = (int)Owner.Creature.Powers.OfType<ColorfulNotePower>().Sum(p => p.Amount);
                return colorful > 4;
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(50m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(combatState)
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(16m);
        }
    }
}