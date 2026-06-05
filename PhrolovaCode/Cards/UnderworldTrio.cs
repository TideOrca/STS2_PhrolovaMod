using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class UnderworldTrio : PhrolovaCard
    {
        public UnderworldTrio() : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("WeakAmount", 3m),
            new DynamicVar("EnhanceAmount", 3m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            decimal weakAmount = DynamicVars["WeakAmount"].BaseValue;
            decimal enhanceAmount = DynamicVars["EnhanceAmount"].BaseValue;

            await PowerCmd.Apply<WeakPower>(Owner.Creature, weakAmount, Owner.Creature, this);
            await PowerCmd.Apply<HecateEnhancePower>(Owner.Creature, enhanceAmount, Owner.Creature, this);

            if (await TryConsumeRebirth())
            {
                var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
                if (tuning != null)
                {
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Red, 1, choiceContext);
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Blue, 1, choiceContext);
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 1, choiceContext);
                }
            }
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}