
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class BetweenDreamAndWaking : PhrolovaCard
    {
        public BetweenDreamAndWaking() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("DreamStacks", 4m)
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };
        
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            var enemies = combatState.GetOpponentsOf(Owner.Creature).ToList();
            if (enemies.Count == 0) return;

            var performing = Owner.Creature.Powers.OfType<PerformingPower>().FirstOrDefault();
            int times = performing != null ? 6 : GetNoteCount();

            var rng = Owner.RunState.Rng.CombatTargets;
            int dreamStacks = (int)DynamicVars["DreamStacks"].BaseValue;

            for (int i = 0; i < times; i++)
            {
                var target = rng.NextItem(enemies);
                if (target != null)
                {
                    await PowerCmd.Apply<DreamMarkPower>(new ThrowingPlayerChoiceContext(), new[] { target }, dreamStacks, Owner.Creature, this, false);
                }
            }
        }

        private int GetNoteCount()
        {
            var tuning = Owner?.Creature?.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning == null) return 0;
            return tuning.GetCurrentTotal();
        }

        protected override void OnUpgrade()
        {
            DynamicVars["DreamStacks"].UpgradeValueBy(2m);
        }
    }
}