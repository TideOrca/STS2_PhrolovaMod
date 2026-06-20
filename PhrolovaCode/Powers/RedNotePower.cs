
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class RedNotePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;   // 改为 Single
    }
}