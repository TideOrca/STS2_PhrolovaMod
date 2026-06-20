
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class FallingIntoDream : PhrolovaCard
    {
        public FallingIntoDream() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("DreamStacks", 15m)
        };
        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["DreamStacks"].BaseValue;
            var combatState = Owner?.Creature?.CombatState;
            if (combatState != null)
            {
                foreach (var enemy in combatState.GetOpponentsOf(Owner.Creature))
                {
                    await PowerCmd.Apply<DreamMarkPower>(new ThrowingPlayerChoiceContext(), new[] { enemy }, stacks, Owner.Creature, this, false);
                }
            }

            // 获得1层重世
            await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["DreamStacks"].UpgradeValueBy(4m); // 15 → 19
        }
    }
}