
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class BrokenString : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        public override async Task BeforeCombatStart()
        {
            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
                await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 1);
        }
    }
}