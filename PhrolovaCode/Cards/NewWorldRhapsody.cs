
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class NewWorldRhapsody : PhrolovaCard
    {
        public NewWorldRhapsody() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 每次打出都施加一个新实例
            await PowerCmd.Apply<NewWorldRhapsodyPower>(Owner.Creature, 1, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1); // 1费 → 0费
        }
    }
}