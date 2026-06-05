
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Dreamscape : PhrolovaCard
    {
        public Dreamscape() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded ? new[] { CardKeyword.Innate } : Enumerable.Empty<CardKeyword>();

        // 动态变量，让卡面描述显示能量图标
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new EnergyVar(1)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await PowerCmd.Apply<DreamscapePower>(Owner.Creature, 1, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Innate);
        }
    }
}