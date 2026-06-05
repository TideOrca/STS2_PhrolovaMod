
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class DreamPhantom : PhrolovaCard
    {
        public DreamPhantom() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? new[] { CardKeyword.Exhaust, CardKeyword.Retain } : new[] { CardKeyword.Exhaust };
        
        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var dream = cardPlay.Target.Powers.OfType<DreamMarkPower>().FirstOrDefault();
            if (dream == null || dream.Amount <= 0) return;

            await CreatureCmd.GainBlock(Owner.Creature, dream.Amount, ValueProp.Unpowered, null);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}