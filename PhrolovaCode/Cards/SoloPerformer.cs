
using Phrolova.PhrolovaCode.Pools;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class SoloPerformer : PhrolovaCard
    {
        public SoloPerformer() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(8m, ValueProp.Move),
            new PowerVar<WeakPower>(1m),
            new CardsVar(2)
        };

        protected override IEnumerable<IHoverTip> AdditionalHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<WeakPower>()
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 基础效果：获得格挡，给予虚弱
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), new[] { cardPlay.Target }, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this, false);

            // 重世效果：抽2张牌
            if (await TryConsumeRebirth())
            {
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(2m); // 格挡8→10
            DynamicVars["WeakPower"].UpgradeValueBy(1m); // 虚弱1→2
        }
    }
}