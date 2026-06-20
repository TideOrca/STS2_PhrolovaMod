
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class HecateExtraAttackPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;  // 可叠加
    }
}