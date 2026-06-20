#nullable enable
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class PerformEchoPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter; // 可叠加层数 = 重放次数

        public override int ModifyCardPlayCount(CardModel card, Creature? target, int playCount)
        {
            if (card.Owner?.Creature != Owner) return playCount;

            // 演奏状态仍然存在才生效（理论上本能力只在演奏时存在）
            if (!Owner.Powers.OfType<PerformingPower>().Any()) return playCount;

            // 限制额外重放次数上限为99
            int extra = Math.Min(Amount, 99);
            return playCount + extra;
        }
    }
}