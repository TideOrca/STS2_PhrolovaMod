
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Prepare : PhrolovaCard
    {
        public Prepare() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new CardsVar(1)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 获得1层重世
            await PowerCmd.Apply<RebirthPower>(Owner.Creature, 1, Owner.Creature, this);
            // 抽牌
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(1m); // 升到2张
        }
    }
}