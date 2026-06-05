
using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class WhiteDress : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Rare;

        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player != Owner) return;
            await PowerCmd.Apply<ResonancePower>(Owner.Creature, 4, Owner.Creature, null);
        }
    }
}