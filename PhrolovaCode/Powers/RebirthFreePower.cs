namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class RebirthFreePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter; // 改为可叠加
        public override bool IsInstanced => false;

        // 只要有该能力，所有重世牌耗能变为0
        public override bool TryModifyEnergyCostInCombat(CardModel card, decimal originalCost, out decimal modifiedCost)
        {
            if (card.Owner?.Creature == Owner && card.CanonicalKeywords.Contains(Keywords.Rebirth))
            {
                modifiedCost = 0;
                return true;
            }
            modifiedCost = originalCost;
            return false;
        }

        // 打出重世牌后，减少一层
        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner?.Creature != Owner) return;
            if (cardPlay.Card.CanonicalKeywords.Contains(Keywords.Rebirth))
            {
                await PowerCmd.Decrement(this); // 层数-1，若降为0则自动移除
            }
        }
    }
}