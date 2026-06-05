using Phrolova.PhrolovaCode.Cards;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class RebirthRenewedBondPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card is FinalMovement && cardPlay.Card.Owner?.Creature == Owner)
            {
                // 每层给予 1 层余响
                await PowerCmd.Apply<ResonancePower>(Owner, Amount, Owner, null);
            }
        }
    }
}