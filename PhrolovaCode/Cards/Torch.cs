
using Phrolova.PhrolovaCode.Powers;


namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Torch : PhrolovaCard
    {
        public Torch() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<TorchPower>(Owner.Creature, 1, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1); // 升级后0费
        }
    }
}