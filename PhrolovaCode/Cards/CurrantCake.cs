
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class CurrantCake : PhrolovaCard
    {
        public CurrantCake() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            IsUpgraded ? new[] { CardKeyword.Innate } : Enumerable.Empty<CardKeyword>();

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 施加免疫虚弱惩罚的能力
            await PowerCmd.Apply<IgnoreWeaknessPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }
    }
}