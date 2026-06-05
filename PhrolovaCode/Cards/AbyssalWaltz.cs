
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class AbyssalWaltz : PhrolovaCard
    {
        public AbyssalWaltz() : base(2, CardType.Power, CardRarity.Ancient, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<AbyssalWaltzPower>(Owner.Creature, 1, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1); 
        }
    }
}