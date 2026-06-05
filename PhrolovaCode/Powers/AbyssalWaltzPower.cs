
using MegaCrit.Sts2.Core.Rooms;

namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class AbyssalWaltzPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter; 
        public override bool IsInstanced => false;

        public override Task AfterCombatEnd(CombatRoom room)
        {
            if (Owner?.Player == null) return Task.CompletedTask;

            int count = Amount;
            for (int i = 0; i < count; i++)
            {
                var upgradeable = Owner.Player.Deck.Cards
                    .Where(c => c.IsUpgradable)
                    .ToList();
                if (upgradeable.Count == 0) break;

                var card = Owner.Player.RunState.Rng.CombatTargets.NextItem(upgradeable);
                CardCmd.Upgrade(card);
            }
            return Task.CompletedTask;
        }
    }
}