
namespace Phrolova.Powers
{
    public sealed class RebirthPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;  // 可叠加层数
    }
}