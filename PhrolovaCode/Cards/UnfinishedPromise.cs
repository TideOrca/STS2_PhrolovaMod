
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class UnfinishedPromise : PhrolovaCard
    {
        public UnfinishedPromise() : base(2, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        // 添加动态变量，仅用于卡面描述渲染能量图标
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new EnergyVar(1)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<UnfinishedPromisePower>(Owner.Creature, 1, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1); // 2费 → 1费
        }
    }
}