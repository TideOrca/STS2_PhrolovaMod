namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class TorchPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;
    }
}