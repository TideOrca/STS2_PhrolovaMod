
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class CommandFormPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        // 每打出一张牌获得 1 层余响
        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner?.Creature != Owner) return;
            await PowerCmd.Apply<ResonancePower>(Owner, 1, Owner, null);
        }
    }
}