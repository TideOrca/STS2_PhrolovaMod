
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class WailRejoice : PhrolovaCard
    {
        public WailRejoice() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? new[] { CardKeyword.Exhaust } : new[] { CardKeyword.Exhaust, CardKeyword.Ethereal };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            // 遍历自身所有可叠加的能力（Counter类型），将其数量翻倍
            foreach (var power in Owner.Creature.Powers.Where(p => p.StackType == PowerStackType.Counter).ToList())
            {
                if (power.Amount > 0)
                {
                    await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), power, power.Amount, Owner.Creature, this, false);
                }
            }
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Ethereal);
        }
    }
}