
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class DeepSlumber : PhrolovaCard
    {
        public DeepSlumber() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };
        
        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var dream = cardPlay.Target.Powers.OfType<DreamMarkPower>().FirstOrDefault();
            if (dream == null || dream.Amount <= 0) return;

            int multiplier = IsUpgraded ? 3 : 2;
            int increase = dream.Amount * (multiplier - 1);
            await PowerCmd.ModifyAmount(new ThrowingPlayerChoiceContext(), dream, increase, Owner.Creature, this, false);
        }

        protected override void OnUpgrade() { }
    }
}