
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class ListenToEchoesPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            // 只对自身亲自获得的余响增加进行扩散，忽略队友扩散来的余响
            if (power is not ResonancePower || power.Owner != Owner || amount <= 0 || applier != Owner)
                return;

            var combatState = Owner.CombatState;
            if (combatState == null) return;

            foreach (var player in combatState.Players.Where(p => p.Creature.IsAlive && p != Owner.Player))
            {
                // 给其他队友施加等量余响，cardSource 传 null（能力不是卡牌）
                await PowerCmd.Apply<ResonancePower>(new ThrowingPlayerChoiceContext(), new[] { player.Creature }, amount, Owner, null, false);
            }
        }
    }
}