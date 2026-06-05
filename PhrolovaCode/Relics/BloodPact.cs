
namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class BloodPact : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        public override decimal ModifyDamageAdditive(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
        {
            if (target == Owner?.Creature && Owner.Creature.HasPower<WeakPower>())
                return -2m;
            return 0m;
        }
    }
}