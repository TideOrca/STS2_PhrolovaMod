
using Phrolova.PhrolovaCode.Cards;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class RebirthBlockPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter; // 每层决定格挡值

        static RebirthBlockPower()
        {
            PhrolovaCard.RebirthTriggered += OnRebirthTriggeredForAll;
        }

        private static async void OnRebirthTriggeredForAll(Creature owner)
        {
            if (owner == null) return;
            var instances = owner.Powers.OfType<RebirthBlockPower>().ToList();
            if (instances.Count == 0) return;

            // 累加所有实例的层数，给予总格挡
            decimal totalBlock = instances.Sum(p => p.Amount);
            if (totalBlock > 0)
                await CreatureCmd.GainBlock(owner, totalBlock, ValueProp.Move, null);
        }
    }
}