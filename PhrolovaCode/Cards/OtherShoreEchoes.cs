

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class OtherShoreEchoes : PhrolovaCard
    {
        public OtherShoreEchoes() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.AllEnemies) { }
        
        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new PowerVar<VulnerablePower>(3m),
            new DynamicVar("RebirthStacks", 3m)
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<VulnerablePower>()
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 基础效果：给予易伤
            var combatState = Owner?.Creature?.CombatState;
            if (combatState != null)
            {
                var enemies = combatState.GetOpponentsOf(Owner.Creature);
                foreach (var enemy in enemies)
                {
                    await PowerCmd.Apply<VulnerablePower>(enemy, DynamicVars["VulnerablePower"].BaseValue, Owner.Creature, this);
                }
            }
            // 获得多层重世
            int rebirthStacks = (int)DynamicVars["RebirthStacks"].BaseValue;
            await PowerCmd.Apply<RebirthPower>(Owner.Creature, rebirthStacks, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["VulnerablePower"].UpgradeValueBy(1m);
            DynamicVars["RebirthStacks"].UpgradeValueBy(1m);
        }
    }
}