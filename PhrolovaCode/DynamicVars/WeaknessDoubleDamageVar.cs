
namespace Phrolova.PhrolovaCode.DynamicVars
{
    public sealed class WeaknessDoubleDamageVar : DynamicVar
    {
        private readonly decimal _baseDamage;
        private readonly ValueProp _props;

        public WeaknessDoubleDamageVar(decimal damage, ValueProp props)
            : base("Damage", damage)
        {
            _baseDamage = damage;
            _props = props;
        }

        // 预览/卡面描述时调用
        public override void UpdateCardPreview(CardModel card, CardPreviewMode previewMode, Creature? target, bool runGlobalHooks)
        {
            decimal damage = _baseDamage;
            if (card?.Owner != null)
            {
                int weakStacks = card.Owner.Creature.GetPowerAmount<WeakPower>();
                int doubleTimes = weakStacks / 4;
                damage *= (decimal)Math.Pow(2, doubleTimes);
            }
            base.PreviewValue = damage;
        }

        // 获取当前实际伤害（供 OnPlay 使用）
        public decimal GetCurrentDamage(CardModel card)
        {
            decimal damage = _baseDamage;
            if (card?.Owner != null)
            {
                int weakStacks = card.Owner.Creature.GetPowerAmount<WeakPower>();
                int doubleTimes = weakStacks / 4;
                damage *= (decimal)Math.Pow(2, doubleTimes);
            }
            return damage;
        }
    }
}