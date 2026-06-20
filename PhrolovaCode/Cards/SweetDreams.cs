
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class SweetDreams : PhrolovaCard
    {
        public SweetDreams() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("DreamStacks", 5m)  // 基础5层，升级后7层
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int stacks = (int)DynamicVars["DreamStacks"].BaseValue;
            await PowerCmd.Apply<SweetDreamsPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, stacks, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["DreamStacks"].UpgradeValueBy(2m); // 5 → 7
        }
    }
}