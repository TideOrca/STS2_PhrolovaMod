
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class ImmortalBody : PhrolovaCard
    {
        public ImmortalBody() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        public override bool GainsBlock => true;

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(15m, ValueProp.Move),
            new EnergyVar(2)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            if (await TryConsumeRebirth())
            {
                await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(5m); // 15 → 20
        }
    }
}