
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class CommandForm : PhrolovaCard
    {
        public CommandForm() : base(3, CardType.Power, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => 
            IsUpgraded ? Enumerable.Empty<CardKeyword>() : new[] { CardKeyword.Ethereal };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<CommandFormPower>(Owner.Creature, 1, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            RemoveKeyword(CardKeyword.Ethereal);
        }
    }
}