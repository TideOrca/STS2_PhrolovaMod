
using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class PerformStrengthPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
        {
            await PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
        }

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player.Creature != Owner) return;
            await PowerCmd.Apply<StrengthPower>(Owner, -Amount, Owner, null);
            await PowerCmd.Remove(this);
        }
    }
}