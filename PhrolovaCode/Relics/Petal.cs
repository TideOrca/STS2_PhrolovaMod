
namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class Petal : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Common;

        public override async Task BeforeCombatStart()
        {
            await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, null, false);
        }
    }
}