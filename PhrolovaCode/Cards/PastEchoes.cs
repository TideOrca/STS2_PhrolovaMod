
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class PastEchoes : PhrolovaCard
    {
        public PastEchoes() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        public override bool GainsBlock => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(10m, ValueProp.Move),
            new EnergyVar(1) // 下回合获得的能量
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 获得格挡
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            // 下回合获得能量（官方能力）
            await PowerCmd.Apply<EnergyNextTurnPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, DynamicVars.Energy.BaseValue, Owner.Creature, this, false);

            // 下回合获得1枚彩乐（自定义能力）
            await PowerCmd.Apply<ColorfulNoteNextTurnPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(3m);        // 10 → 13
            DynamicVars.Energy.UpgradeValueBy(1m);       // 1 → 2
        }
    }
}