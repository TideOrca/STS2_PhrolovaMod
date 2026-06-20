
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class RecordPlayer : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Shop;

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            if (power is DreamMarkPower && amount > 0 && applier == Owner?.Creature)
            {
                await CreatureCmd.GainBlock(Owner.Creature, 4, ValueProp.Unpowered, null);
            }
        }
    }
}