using MegaCrit.Sts2.Core.Models.Enchantments;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class RhapsodyTicket : PhrolovaCard
    {
        public RhapsodyTicket() : base(1, CardType.Skill, CardRarity.Rare, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords =>
            IsUpgraded
                ? new[] { CardKeyword.Exhaust, CardKeyword.Retain }
                : new[] { CardKeyword.Exhaust };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null) return;

            // 攻击牌通用附魔池
            var attackEnchantments = new List<EnchantmentModel>
            {
                ModelDb.Enchantment<Adroit>(),
                ModelDb.Enchantment<Corrupted>(),
                ModelDb.Enchantment<Glam>(),
                ModelDb.Enchantment<Goopy>(),
                ModelDb.Enchantment<Imbued>(),
                ModelDb.Enchantment<Inky>(),
                ModelDb.Enchantment<Instinct>(),
                ModelDb.Enchantment<Momentum>(),
                ModelDb.Enchantment<PerfectFit>(),
                ModelDb.Enchantment<Sharp>(),
                ModelDb.Enchantment<Slither>(),
                ModelDb.Enchantment<Sown>(),
                ModelDb.Enchantment<Spiral>(),
                ModelDb.Enchantment<Steady>(),
                ModelDb.Enchantment<Swift>(),
                ModelDb.Enchantment<TezcatarasEmber>(),
                ModelDb.Enchantment<Vigorous>()
            };

            // 技能/能力牌通用附魔池（排除攻击专属附魔）
            var skillEnchantments = new List<EnchantmentModel>
            {
                ModelDb.Enchantment<Adroit>(),
                ModelDb.Enchantment<Corrupted>(),
                ModelDb.Enchantment<Imbued>(),
                ModelDb.Enchantment<Instinct>(),
                ModelDb.Enchantment<Momentum>(),
                ModelDb.Enchantment<Nimble>(),
                ModelDb.Enchantment<PerfectFit>(),
                ModelDb.Enchantment<RoyallyApproved>(),
                ModelDb.Enchantment<SlumberingEssence>(),
                ModelDb.Enchantment<SoulsPower>(),
                ModelDb.Enchantment<Steady>(),
                ModelDb.Enchantment<Swift>(),
                ModelDb.Enchantment<Vigorous>()
            };

            var hand = PileType.Hand.GetPile(Owner);
            var enchantableCards = hand.Cards
                .Where(c => c.Enchantment == null)
                .ToList();
            
            foreach (var card in enchantableCards)
            {
                // 根据卡牌类型选择对应的附魔池
                var pool = card.Type == CardType.Attack ? attackEnchantments : skillEnchantments;

                EnchantmentModel? enchantMutable = null;
                for (int attempt = 0; attempt < 10; attempt++)
                {
                    var enchantCanonical = Owner.RunState.Rng.CombatCardGeneration.NextItem(pool);
                    var candidate = (EnchantmentModel)enchantCanonical.MutableClone();
                    if (candidate.CanEnchant(card))
                    {
                        enchantMutable = candidate;
                        break;
                    }
                }

                if (enchantMutable != null)
                {
                    CardCmd.Enchant(enchantMutable, card, 1);
                }
            }

            await Task.CompletedTask;
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}