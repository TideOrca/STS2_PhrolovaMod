
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class UnfinishedOpus : PhrolovaCard
    {
        public UnfinishedOpus() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<HecateExtraAttackPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1); // 1费 → 0费
        }
    }
}