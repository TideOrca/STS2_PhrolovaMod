

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class BloodAndTears : PhrolovaCard
    {
        public BloodAndTears() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        public override bool GainsBlock => true;

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(20m, ValueProp.Move),
            new HealVar(10m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 失去生命值
            await CreatureCmd.Damage(choiceContext, Owner.Creature, DynamicVars.Heal.BaseValue, 
                ValueProp.Unblockable | ValueProp.Unpowered, Owner.Creature, this);
            // 获得格挡
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            // 重世效果：回复等量生命
            if (await TryConsumeRebirth())
            {
                await CreatureCmd.Heal(Owner.Creature, DynamicVars.Heal.BaseValue);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(10m); // 20 → 30
        }
    }
}