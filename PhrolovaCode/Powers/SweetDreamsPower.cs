
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class SweetDreamsPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            // 只有弗洛洛自己施加的虚弱才触发，且目标必须是敌人
            if (power is WeakPower && amount > 0 && applier == Owner)
            {
                var target = power.Owner;
                if (target != null && target.IsEnemy)
                {
                    // Amount 是能力层数，每层代表给予的迷梦层数
                    await PowerCmd.Apply<DreamMarkPower>(new ThrowingPlayerChoiceContext(), new[] { target }, Amount, Owner, null, false);
                }
            }
        }
    }
}