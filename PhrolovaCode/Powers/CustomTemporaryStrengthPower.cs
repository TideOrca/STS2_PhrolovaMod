using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Powers
{
    public abstract class CustomTemporaryStrengthPower : PhrolovaPower
    {
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player.Creature == Owner)
                await PowerCmd.Remove(this);
        }
    }
}