
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class NoMoreSunsetPower : PhrolovaPower, ICustomModel
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;

        public int HealPercent { get; set; } = 20;

        public override bool ShouldDieLate(Creature creature)
        {
            if (creature != Owner) return true;
            if (creature.CurrentHp > 0) return true;

            _ = HealAndRemove();
            return false;
        }

        private async Task HealAndRemove()
        {
            if (Owner == null || Owner.Player == null) return;
            decimal healAmount = Math.Max(1m, Owner.MaxHp * (HealPercent / 100m));
            await CreatureCmd.Heal(Owner, healAmount);
            
            // 从战斗中移除一张实例（手/抽/弃/消耗堆）
            var combatCard = PileType.Hand.GetPile(Owner.Player)?.Cards.OfType<Cards.NoMoreSunset>().FirstOrDefault()
                             ?? PileType.Draw.GetPile(Owner.Player)?.Cards.OfType<Cards.NoMoreSunset>().FirstOrDefault()
                             ?? PileType.Discard.GetPile(Owner.Player)?.Cards.OfType<Cards.NoMoreSunset>().FirstOrDefault()
                             ?? PileType.Exhaust.GetPile(Owner.Player)?.Cards.OfType<Cards.NoMoreSunset>().FirstOrDefault();
            if (combatCard != null)
                await CardPileCmd.RemoveFromCombat(combatCard);
            
            var card = Owner.Player.Deck.Cards.OfType<Cards.NoMoreSunset>().FirstOrDefault();
            if (card != null)
            {
                await CardPileCmd.RemoveFromDeck(card, showPreview: false);
            }
            await PowerCmd.Remove(this);
        }
    }
}