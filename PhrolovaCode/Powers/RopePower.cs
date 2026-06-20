using Phrolova.PhrolovaCode.Cards;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class RopePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        // 每层增加 1% 伤害
        public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
        {
            if (cardSource is FinalMovement && dealer == Owner)
                return 1m + 0.01m * Amount;   // 每层 +1%
            return 1m;
        }
    }
}