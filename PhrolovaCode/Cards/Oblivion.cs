
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Oblivion : PhrolovaCard
    {
        public Oblivion() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(28m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            ArgumentNullException.ThrowIfNull(cardPlay.Target, "cardPlay.Target");

            if ((await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .FromCard(this)
                    .Targeting(cardPlay.Target)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext)).Results.Any(r => r.WasTargetKilled))
            {
                var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
                if (tuning != null)
                {
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 2, choiceContext);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(8m); // 28 → 36
        }
    }
}