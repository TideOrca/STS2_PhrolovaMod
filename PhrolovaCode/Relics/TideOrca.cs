
namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class TideOrca : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Event;
    }
}