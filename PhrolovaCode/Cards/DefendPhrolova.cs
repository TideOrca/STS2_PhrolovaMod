
using Phrolova.PhrolovaCode.Cards;

namespace Phrolova.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class DefendPhrolova : PhrolovaCard
    {
        public DefendPhrolova() : base(1, CardType.Skill, CardRarity.Basic, TargetType.Self) { }

        public override bool GainsBlock => true;

        protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Defend };

        protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new BlockVar(5m, ValueProp.Move) };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
        }

        protected override void OnUpgrade() => DynamicVars.Block.UpgradeValueBy(3m);
    }
}