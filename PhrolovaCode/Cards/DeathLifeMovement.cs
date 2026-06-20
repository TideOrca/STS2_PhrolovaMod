using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class DeathLifeMovement : PhrolovaCard
    {
        public DeathLifeMovement() : base(1, CardType.Attack, CardRarity.Ancient, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(6m, ValueProp.Move),
            new DynamicVar("Hits", 3m),
            new PowerVar<WeakPower>(3m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int hits = (int)DynamicVars["Hits"].BaseValue;

            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(hits)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 施加虚弱
            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), new[] { cardPlay.Target }, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this, false);

            // 获得弦乐
            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
                await tuning.ForceAddNote(TuningStatePower.NoteType.Red, 1, choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Hits"].UpgradeValueBy(2m);         // 3 → 5
            DynamicVars["WeakPower"].UpgradeValueBy(2m);    // 3 → 5
        }
    }
}