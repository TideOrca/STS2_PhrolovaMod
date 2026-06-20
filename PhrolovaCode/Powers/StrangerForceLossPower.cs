using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class StrangerForceLossPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;

        public override async Task AfterApplied(Creature? applier, CardModel? cardSource)
        {
            // 施加时扣除力量
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner }, -Amount, applier ?? Owner, cardSource, false);
        }

        // 玩家回合开始时恢复敌人力量（等同于敌人回合结束，因为 AfterTurnEnd 签名已变更）
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || Owner.IsDead) return;
            if (Owner.Side != CombatSide.Enemy) return;

            // 恢复之前扣减的力量，然后移除自身
            await PowerCmd.Apply<StrengthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner }, Amount, Owner, null, false);
            await PowerCmd.Remove(this);
        }
    }
}