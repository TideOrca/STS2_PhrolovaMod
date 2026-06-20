using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Powers
{
    public abstract class CustomTemporaryDexterityPower : PhrolovaPower
    {
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player.Creature == Owner)
                await PowerCmd.Remove(this);
        }
    }
}