namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class ColorfulNotePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;   // 改为 Single
    }
}