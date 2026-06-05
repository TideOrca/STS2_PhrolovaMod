
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class StrangerForceLossPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
        {
            // 施加时扣除力量（力量允许负层数）
            await PowerCmd.Apply<StrengthPower>(Owner, -Amount, applier ?? Owner, cardSource);
        }

        // 在敌人回合结束时恢复力量并移除自身
        public override async Task AfterTurnEnd(PlayerChoiceContext choiceContext, CombatSide side)
        {
            if (side != CombatSide.Enemy) return;
            if (Owner == null || Owner.Side != CombatSide.Enemy) return;

            // 恢复减掉的力量
            await PowerCmd.Apply<StrengthPower>(Owner, Amount, Owner, null);
            await PowerCmd.Remove(this);
        }
    }
}