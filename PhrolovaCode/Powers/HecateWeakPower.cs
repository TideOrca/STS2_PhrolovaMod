
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class HecateWeakPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Counter;
        public override bool IsInstanced => false;

        public override async Task AfterDamageGiven(PlayerChoiceContext choiceContext, Creature dealer, DamageResult result, ValueProp props, Creature target, CardModel cardSource)
        {
            if (dealer != Owner?.Player?.Creature?.Pets.FirstOrDefault(p => p.Monster is Hecate))
                return;

            // 赫卡忒造成伤害时，给予目标虚弱（层数 = 本能力层数，每层1虚弱）
            await PowerCmd.Apply<WeakPower>(target, Amount, dealer, cardSource);
        }
    }
}