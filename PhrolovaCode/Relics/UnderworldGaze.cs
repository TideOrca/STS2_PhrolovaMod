
namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class UnderworldGaze : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        // 效果由 PerformingPower.GetHecateDamage 触发
        // 无额外代码
    }
}