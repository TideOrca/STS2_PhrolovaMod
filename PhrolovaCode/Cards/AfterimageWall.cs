
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class AfterimageWall : PhrolovaCard
    {
        public AfterimageWall() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(7m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 基础效果
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            // 自身处于虚弱时，额外获得等量格挡
            if (Owner.Creature.HasPower<WeakPower>())
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);
        }
    }
}