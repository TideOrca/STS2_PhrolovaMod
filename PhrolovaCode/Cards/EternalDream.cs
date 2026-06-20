
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class EternalDream : PhrolovaCard
    {
        public EternalDream() : base(1, CardType.Skill, CardRarity.Ancient, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(4m, ValueProp.Move),
            new DynamicVar("Times", 3m),
            new DynamicVar("RebirthStacks", 3m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int times = (int)DynamicVars["Times"].BaseValue;
            for (int i = 0; i < times; i++)
            {
                await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
            }

            int rebirthStacks = (int)DynamicVars["RebirthStacks"].BaseValue;
            await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, rebirthStacks, Owner.Creature, this, false);

            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
                await tuning.ForceAddNote(TuningStatePower.NoteType.Blue, 1, choiceContext);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Times"].UpgradeValueBy(2m);            // 3 → 5
            DynamicVars["RebirthStacks"].UpgradeValueBy(1m);    // 3 → 4
        }
    }
}