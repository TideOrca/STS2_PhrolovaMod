
using Phrolova.PhrolovaCode.Powers;


namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Disorder : PhrolovaCard
    {
        public Disorder() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("Enhance", 4m) 
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["Enhance"].BaseValue;
            await PowerCmd.Apply<HecateEnhancePower>(Owner.Creature, stacks, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Enhance"].UpgradeValueBy(2m); 
        }
    }
}