
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Prologue : PhrolovaCard
    {
        public Prologue() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Innate, CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("RebirthStacks", 1m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["RebirthStacks"].BaseValue;
            await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, stacks, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["RebirthStacks"].UpgradeValueBy(1m);
        }
    }
}