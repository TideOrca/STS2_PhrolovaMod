
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class EphemeralDream : PhrolovaCard
    {
        public EphemeralDream() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            new[] { CardKeyword.Innate, CardKeyword.Exhaust };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            var combatState = Owner.Creature.CombatState;
            if (combatState == null) return;

            // 获取所有角色卡池的卡牌（排除基础牌和永恒牌）
            var allPools = ModelDb.AllCharacterCardPools
                .SelectMany(p => p.AllCards)
                .Where(c => c.Rarity != CardRarity.Basic && !c.CanonicalKeywords.Contains(CardKeyword.Eternal))
                .ToList();

            if (allPools.Count == 0) return;

            // 获取玩家所有战斗内卡牌
            var allCombatCards = Owner.PlayerCombatState.AllCards.ToList();

            var rng = Owner.RunState.Rng.CombatCardSelection;

            foreach (var card in allCombatCards)
            {
                // 跳过自身
                if (card == this) continue;

                // 随机选一个不同的卡牌
                var candidates = allPools.Where(c => c.Id != card.Id).ToList();
                if (candidates.Count == 0) continue;

                var replacementCanonical = candidates[rng.NextInt(candidates.Count)];
                // 使用 CombatState.CreateCard 确保注册到战斗
                var replacement = combatState.CreateCard(replacementCanonical, Owner);

                // 升级效果：如果卡牌已升级，则替换牌也升级
                if (IsUpgraded)
                    CardCmd.Upgrade(replacement);

                await CardCmd.Transform(card, replacement, CardPreviewStyle.HorizontalLayout);
            }
        }

        protected override void OnUpgrade() { }
    }
}