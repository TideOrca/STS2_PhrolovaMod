
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class GuidingBreath : PhrolovaCard
    {
        public GuidingBreath() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("DreamMarkStacks", 8m)
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["DreamMarkStacks"].BaseValue;
            await PowerCmd.Apply<DreamMarkPower>(new ThrowingPlayerChoiceContext(), new[] { cardPlay.Target }, stacks, Owner.Creature, this, false);

            // 获得1层重世
            await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["DreamMarkStacks"].UpgradeValueBy(3m); // 8 → 11
        }
    }
}