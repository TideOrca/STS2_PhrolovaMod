
using MegaCrit.Sts2.Core.CardSelection;
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class OtherShore : PhrolovaCard
    {
        public OtherShore() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? new[] { CardKeyword.Retain } : Enumerable.Empty<CardKeyword>();

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new EnergyVar(1)  // 动态能量，用于描述显示
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            var hand = PileType.Hand.GetPile(Owner);
            if (hand == null || hand.Cards.Count == 0) return;

            var selected = (await CardSelectCmd.FromHand(
                choiceContext,
                Owner,
                new CardSelectorPrefs(CardSelectorPrefs.ExhaustSelectionPrompt, 1),
                card => card != this,
                this
            )).FirstOrDefault();

            if (selected == null) return;

            await CardCmd.Exhaust(choiceContext, selected);

            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            switch (selected.Type)
            {
                case CardType.Attack:
                    if (tuning != null) await tuning.ForceAddNote(TuningStatePower.NoteType.Red, 1, choiceContext);
                    break;
                case CardType.Skill:
                    if (tuning != null) await tuning.ForceAddNote(TuningStatePower.NoteType.Blue, 1, choiceContext);
                    break;
                case CardType.Power:
                    if (tuning != null) await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 1, choiceContext);
                    break;
                case CardType.Status:
                    await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
                    break;
                case CardType.Curse:
                    await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
                    break;
            }
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain);
        }
    }
}