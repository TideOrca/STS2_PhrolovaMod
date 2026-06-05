
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Hush : PhrolovaCard
    {
        // ──────────── 私有嵌套变量类（不创建新文件）────────────
        private class HushDamageVar : DamageVar
        {
            private readonly decimal _originalBaseDamage;

            public HushDamageVar(decimal baseDamage, ValueProp props)
                : base(baseDamage, props)
            {
                _originalBaseDamage = baseDamage;
            }

            public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
            {
                // 从卡牌持有者身上获取 HushPower 的当前倍数（无则为 1）
                var hush = card.Owner?.Creature?.Powers.OfType<HushPower>().FirstOrDefault();
                decimal multiplier = hush?.Amount ?? 1m;

                // 设置基础伤害 = 原始伤害 × 倍数
                BaseValue = _originalBaseDamage * multiplier;

                // 让基类 DamageVar 继续处理力量、虚弱、易伤等修正
                base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
            }
        }
        // ─────────────────────────────────────────────────

        private HushDamageVar _damageVar;

        public Hush() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars
        {
            get
            {
                _damageVar = new HushDamageVar(12m, ValueProp.Move);
                return new DynamicVar[] { _damageVar };
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // BaseValue 已自动包含了 HushPower 的倍数
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 重世效果：施加或翻倍 HushPower
            if (await TryConsumeRebirth())
            {
                var hush = Owner.Creature.Powers.OfType<HushPower>().FirstOrDefault();
                if (hush == null)
                {
                    await PowerCmd.Apply<HushPower>(Owner.Creature, 2, Owner.Creature, this);
                }
                else
                {
                    await PowerCmd.ModifyAmount(hush, hush.Amount, Owner.Creature, this);
                }
            }
        }

        protected override void OnUpgrade()
        {
            // 升级时提升原始伤害 12 → 16
            _damageVar?.UpgradeValueBy(4m);
        }
    }
}