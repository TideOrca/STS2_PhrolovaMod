
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class DryTogether : PhrolovaCard
    {
        public DryTogether() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new PowerVar<WeakPower>(9m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 给自己施加虚弱
            await PowerCmd.Apply<WeakPower>(Owner.Creature, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);

            // 给所有敌人施加等量虚弱
            var combatState = Owner?.Creature?.CombatState;
            if (combatState != null)
            {
                foreach (var enemy in combatState.GetOpponentsOf(Owner.Creature))
                {
                    await PowerCmd.Apply<WeakPower>(enemy, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this);
                }
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1); // 1费 → 0费
        }
    }
}