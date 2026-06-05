
using Phrolova.PhrolovaCode.Pools;
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class NewPathGuidance : PhrolovaCard
    {
        public NewPathGuidance() : base(0, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
        
        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };
        
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new EnergyVar(1),
            new CardsVar(1)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 获得1枚彩乐
            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
                await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 1, choiceContext);

            // 获得1层重世
            await PowerCmd.Apply<RebirthPower>(Owner.Creature, 1, Owner.Creature, this);

            // 获得1费
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);

            // 抽1张牌
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1m); // 抽牌1→2
        }
    }
}