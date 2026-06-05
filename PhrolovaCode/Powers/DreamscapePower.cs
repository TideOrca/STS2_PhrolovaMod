
using MegaCrit.Sts2.Core.Entities.Players;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class DreamscapePower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter; // 可叠加，每层+1能量上限
        public override bool IsInstanced => false;

        // 每回合开始时给自己 1 层虚弱
        public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
        {
            if (player.Creature != Owner) return;
            await PowerCmd.Apply<WeakPower>(Owner, 1, Owner, null);
        }

        // 增加能量上限（模仿 PyrePower）
        public override decimal ModifyMaxEnergy(Player player, decimal amount)
        {
            if (player != Owner?.Player) return amount;
            return amount + Amount; // 每层+1能量上限
        }
    }
}