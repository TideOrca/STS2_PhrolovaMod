
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class NoMoreSunset : PhrolovaCard
    {
        public NoMoreSunset() : base(-1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Unplayable };
        public override bool CanBeGeneratedInCombat => false;

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("HealPercent", 20m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 永远不会被手动打出
        }

        public override void AfterCreated()
        {
            base.AfterCreated();
            ApplyPowerIfNeeded();
        }

        public override async Task AfterCardChangedPiles(CardModel card, PileType oldPileType, AbstractModel source)
        {
            await base.AfterCardChangedPiles(card, oldPileType, source);
            ApplyPowerIfNeeded();
        }

        private void ApplyPowerIfNeeded()
        {
            if (Owner == null || Owner.Creature == null) return;
            if (Owner.Creature.CombatState == null) return; // 仅战斗中生效
            if (Owner.Creature.Powers.OfType<NoMoreSunsetPower>().Any()) return;

            // 通过 ModelDb 获取典范 → 克隆 → 设置百分比
            var canonical = ModelDb.Power<NoMoreSunsetPower>();
            var mutable = (NoMoreSunsetPower)canonical.MutableClone();
            mutable.HealPercent = IsUpgraded ? 30 : 20;
            _ = PowerCmd.Apply<NoMoreSunsetPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, null, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["HealPercent"].UpgradeValueBy(10m);
        }
    }
}