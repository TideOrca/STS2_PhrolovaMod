using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Passing : PhrolovaCard
    {
        public Passing() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("BlockPerRebirth", 4m) // 基础4层，即消耗1重世获得4格挡
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["BlockPerRebirth"].BaseValue;
            await PowerCmd.Apply<RebirthBlockPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, stacks, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["BlockPerRebirth"].UpgradeValueBy(2m); // 升级后6层
        }
    }
}