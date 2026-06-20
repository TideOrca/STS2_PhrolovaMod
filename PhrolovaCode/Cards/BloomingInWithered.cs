namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class BloomingInWithered : PhrolovaCard
    {
        public BloomingInWithered() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Retain };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(16m, ValueProp.Move),
            new DynamicVar("Growth", 4m)   // 每次虚弱伤害成长值
        };

        // 监听虚弱获取
        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power is WeakPower && amount > 0 && applier == Owner?.Creature && Pile?.Type == PileType.Hand)
            {
                decimal growth = DynamicVars["Growth"].BaseValue;
                DynamicVars.Damage.BaseValue += amount * growth;
            }
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            decimal currentDamage = DynamicVars.Damage.BaseValue;

            await DamageCmd.Attack(currentDamage)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            DynamicVars.Damage.BaseValue = 16m;  // 重置
        }

        protected override void OnUpgrade()
        {
            DynamicVars["Growth"].UpgradeValueBy(2m);  // 4 → 6
        }
    }
}