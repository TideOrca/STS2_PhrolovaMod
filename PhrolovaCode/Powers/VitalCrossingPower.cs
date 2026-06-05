
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class VitalCrossingPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override bool IsInstanced => false;
    }
}