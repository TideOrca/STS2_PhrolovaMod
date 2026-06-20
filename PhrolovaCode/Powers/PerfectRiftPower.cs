
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class PerfectRiftPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;

        public override decimal ModifyDamageMultiplicative(Creature target, decimal amount, ValueProp props, Creature dealer, CardModel cardSource)
        {
            if (!props.IsPoweredAttack()) return 1m;
            if (cardSource == null) return 1m;
            if (dealer != Owner) return 1m; 
            if (!Owner.HasPower<WeakPower>()) return 1m;

            return 2m;
        }
    }
}