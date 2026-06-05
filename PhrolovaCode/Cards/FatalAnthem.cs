
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class FatalAnthem : PhrolovaCard
    {
        // ──────── 自定义伤害变量：基础 + 虚弱层数×额外伤害 ────────
        private class WeaknessBonusDamageVar : DamageVar
        {
            private readonly decimal _baseDamage;
            public decimal ExtraPerWeak { get; set; }

            public WeaknessBonusDamageVar(decimal baseDamage, decimal extraPerWeak, ValueProp props)
                : base(baseDamage, props)
            {
                _baseDamage = baseDamage;
                ExtraPerWeak = extraPerWeak;
            }

            public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
            {
                // 获取目标身上的虚弱层数
                int weakStacks = target?.GetPower<WeakPower>()?.Amount ?? 0;
                // 基础伤害 = 基础 + 额外伤害×虚弱层数
                BaseValue = _baseDamage + ExtraPerWeak * weakStacks;

                // 调用基类预览，让力量/易伤等修正自动生效
                base.UpdateCardPreview(card, previewMode, target, runGlobalHooks);
            }
        }

        private WeaknessBonusDamageVar _damageVar;

        public FatalAnthem() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars
        {
            get
            {
                _damageVar = new WeaknessBonusDamageVar(9m, 2m, ValueProp.Move); // 基础9，每层虚弱+2
                return new DynamicVar[] { _damageVar };
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 直接使用动态变量计算好的伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade()
        {
            if (_damageVar != null)
                _damageVar.ExtraPerWeak += 1m; // 每层虚弱额外伤害 2 → 3
        }
    }
}