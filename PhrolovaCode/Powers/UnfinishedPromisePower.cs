
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class UnfinishedPromisePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        static UnfinishedPromisePower()
        {
            TuningStatePower.NoteGained += OnNoteGainedForAll;
        }

        private static async void OnNoteGainedForAll(Creature owner, TuningStatePower.NoteType type)
        {
            if (owner?.Player == null) return;
            var instances = owner.Powers.OfType<UnfinishedPromisePower>().ToList();
            int totalEnergy = instances.Sum(p => p.Amount);
            if (totalEnergy > 0)
            {
                await PlayerCmd.GainEnergy(totalEnergy, owner.Player);
            }
        }
    }
}