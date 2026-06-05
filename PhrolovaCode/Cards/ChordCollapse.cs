
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class ChordCollapse : PhrolovaCard
    {
        public ChordCollapse() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(2m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int totalNotes;

            var performing = Owner.Creature.Powers.OfType<PerformingPower>().FirstOrDefault();
            if (performing != null)
            {
                totalNotes = 6;
            }
            else
            {
                totalNotes = Owner.Creature.Powers.OfType<RedNotePower>().Count()
                             + Owner.Creature.Powers.OfType<BlueNotePower>().Count()
                             + Owner.Creature.Powers.OfType<ColorfulNotePower>().Count();
            }

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(totalNotes)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(1m);
        }
    }
}