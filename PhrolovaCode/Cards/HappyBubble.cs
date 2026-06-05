
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Nodes.CommonUi;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class HappyBubble : PhrolovaCard
    {
        public HappyBubble() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("SelectionCount", 3m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            var combatState = Owner.Creature.CombatState;
            if (combatState == null) return;

            // 获取弗洛洛自己的卡池（所有已解锁的卡牌），排除基础牌、永恒牌、先古牌和多人牌
            var ownPool = Owner.Character.CardPool
                .GetUnlockedCards(Owner.UnlockState, Owner.RunState.CardMultiplayerConstraint)
                .Where(c => c.Rarity != CardRarity.Basic
                    && !c.CanonicalKeywords.Contains(CardKeyword.Eternal)
                    && c.Rarity != CardRarity.Ancient                          // 排除先古牌
                    && c.MultiplayerConstraint != CardMultiplayerConstraint.MultiplayerOnly) // 排除多人牌
                .ToList();

            if (ownPool.Count == 0) return;

            // 从抽牌堆选择卡牌
            int selectCount = (int)DynamicVars["SelectionCount"].BaseValue;
            var drawPile = PileType.Draw.GetPile(Owner);
            if (drawPile.Cards.Count == 0) return;

            var prefs = new CardSelectorPrefs(SelectionScreenPrompt, selectCount);
            var selectedCards = await CardSelectCmd.FromSimpleGrid(choiceContext, drawPile.Cards, Owner, prefs);

            var rng = Owner.RunState.Rng.CombatCardSelection;

            foreach (var card in selectedCards)
            {
                var candidates = ownPool.Where(c => c.Id != card.Id).ToList();
                if (candidates.Count == 0) continue;

                var replacementCanonical = candidates[rng.NextInt(candidates.Count)];
                var replacement = combatState.CreateCard(replacementCanonical, Owner);

                if (IsUpgraded)
                    CardCmd.Upgrade(replacement);

                await CardCmd.Transform(card, replacement, CardPreviewStyle.HorizontalLayout);
            }
        }

        protected override void OnUpgrade() { }
    }
}