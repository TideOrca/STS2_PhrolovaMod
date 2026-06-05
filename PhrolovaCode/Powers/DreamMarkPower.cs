
using BaseLib.Hooks;
using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class DreamMarkPower : PhrolovaPower, IHealthBarForecastSource
    {
        public override PowerType Type => PowerType.Debuff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        private bool _overHpStunApplied; // 用于瞬时眩晕标记

        // 进度条（深红色，从左侧开始）
        public IEnumerable<HealthBarForecastSegment> GetHealthBarForecastSegments(HealthBarForecastContext context)
        {
            if (Owner == null || Owner.IsDead || Amount <= 0)
                yield break;

            yield return new HealthBarForecastSegment(
                Amount,
                new Color("8B0000"),
                HealthBarForecastDirection.FromLeft,
                order: 0
            );
        }

        // 层数变化时：削减力量 + 检查是否首次超过生命值（立即眩晕）
        public override async Task AfterPowerAmountChanged(PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power != this || Owner == null || Owner.IsDead) return;

            int previousStacks = (int)(Amount - amount); // 变化前的层数
            int newStacks = Amount;

            // 力量削减：每跨越一个10的倍数（10,20,30...）减1力量
            if (amount > 0)
            {
                int prevThreshold = previousStacks / 10 * 10;
                int currThreshold = newStacks / 10 * 10;
                int thresholdsPassed = (currThreshold - prevThreshold) / 10;

                if (thresholdsPassed > 0)
                {
                    await PowerCmd.Apply<StrengthPower>(Owner, -thresholdsPassed, Owner, null);
                }
            }

            // 瞬时眩晕：首次超过生命值时立即眩晕
            int currentHp = Owner.CurrentHp;
            if (newStacks > currentHp)
            {
                if (!_overHpStunApplied)
                {
                    _overHpStunApplied = true;
                    await CreatureCmd.Stun(Owner);
                }
            }
            else // 层数小于等于生命值，重置标记，允许下次超过时再次触发
            {
                _overHpStunApplied = false;
            }
        }

        // 玩家回合开始时：若层数仍大于生命值，再次眩晕（确保回合内无法行动）
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (Owner == null || Owner.IsDead) return;
            if (Amount > Owner.CurrentHp)
                await CreatureCmd.Stun(Owner);
        }
    }
}