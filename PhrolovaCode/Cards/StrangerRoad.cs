
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class StrangerRoad : PhrolovaCard
    {
        public StrangerRoad() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.None) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? Enumerable.Empty<CardKeyword>() : new[] { CardKeyword.Exhaust };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            int weakStacks = Owner.Creature.GetPowerAmount<WeakPower>();
            if (weakStacks <= 0) return;

            foreach (var enemy in combatState.GetOpponentsOf(Owner.Creature))
            {
                await PowerCmd.Apply<StrangerForceLossPower>(new ThrowingPlayerChoiceContext(), new[] { enemy }, weakStacks, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Exhaust);
        }
    }
}