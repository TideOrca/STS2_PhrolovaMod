
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Cycle936 : PhrolovaCard
    {
        public Cycle936() : base(0, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(4m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        // 官方的能力层数变化钩子
        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature applier, CardModel cardSource)
        {
            // 仅当自己施予虚弱且卡牌仍在战斗中时触发
            if (power is not WeakPower || amount <= 0 || applier != Owner?.Creature) return;
            if (HasBeenRemovedFromState) return;

            // 如果卡牌已经在手牌中，不需要再添加
            if (Pile?.Type == PileType.Hand) return;

            // 从弃牌堆/抽牌堆/消耗堆飞回手牌
            await CardPileCmd.Add(this, PileType.Hand);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m); 
        }
    }
}