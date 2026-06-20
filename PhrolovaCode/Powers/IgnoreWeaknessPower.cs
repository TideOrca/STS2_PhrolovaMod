
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class IgnoreWeaknessPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        // 伤害修正逻辑已移至 Harmony 补丁，此处只作为标记
    }
}