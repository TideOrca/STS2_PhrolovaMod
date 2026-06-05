
using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class ColorfulNoteNextTurnPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override bool IsInstanced => false;

        public override async Task AfterPlayerTurnStartEarly(PlayerChoiceContext choiceContext, Player player)
        {
            if (player.Creature != Owner) return;

            var tuning = Owner.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
                await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 1, choiceContext);

            await PowerCmd.Remove(this);
        }
    }
}