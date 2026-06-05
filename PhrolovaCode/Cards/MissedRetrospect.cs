

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class MissedRetrospect : PhrolovaCard
    {
        public MissedRetrospect() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new BlockVar(5m, ValueProp.Move),
            new DynamicVar("UpgradeBase", 2m)   // 基础升级张数
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 1. 基础效果：获得格挡
            await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);

            // 2. 随机升级抽牌堆
            int upgradeCount = (int)DynamicVars["UpgradeBase"].BaseValue;
            UpgradeRandomCards(upgradeCount);

            // 3. 重世效果：消耗一层 → 额外获得2次格挡（每次等于基础格挡值）
            if (await TryConsumeRebirth())
            {
                for (int i = 0; i < 2; i++)
                {
                    await CreatureCmd.GainBlock(Owner.Creature, DynamicVars.Block, cardPlay);
                }
            }
        }

        private void UpgradeRandomCards(int count)
        {
            var drawPile = PileType.Draw.GetPile(Owner).Cards
                .Where(c => c.IsUpgradable)
                .ToList();

            if (drawPile.Count == 0) return;
            var rng = Owner.RunState.Rng.CombatTargets;

            for (int i = 0; i < count && drawPile.Count > 0; i++)
            {
                var card = rng.NextItem(drawPile);
                CardCmd.Upgrade(card);
                drawPile.Remove(card);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Block.UpgradeValueBy(1m);
            DynamicVars["UpgradeBase"].UpgradeValueBy(1m);
        }
    }
}