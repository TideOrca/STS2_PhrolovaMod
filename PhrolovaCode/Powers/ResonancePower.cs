
using Phrolova.PhrolovaCode.Cards;

namespace Phrolova.Powers
{
    public sealed class ResonancePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;
        public override bool AllowNegative => false;

        public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
        {
            if (!props.IsPoweredAttack()) return 1m;
            if (cardSource == null) return 1m;
            
            if (cardSource is FinalMovement)
                return 1m + 0.10m * Amount;
            if (dealer == Owner)
                return 1m + 0.05m * Amount;
            return 1m;
        }
    }
}