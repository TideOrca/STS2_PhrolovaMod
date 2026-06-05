
namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class TornScore : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Uncommon;

        // 效果由 PerformingPower.ApplyNoteEffects 触发
        // 无额外代码
    }
}