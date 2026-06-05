
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class ResonanceStrike : PhrolovaCard
    {
        public ResonanceStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Strike };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(2m, ValueProp.Move),
            new RepeatVar(3)   // 官方多次攻击变量
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .WithHitCount(DynamicVars.Repeat.IntValue)  // 使用 RepeatVar 的次数
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            // 整张牌只加一次余响
            await PowerCmd.Apply<ResonancePower>(Owner.Creature, 1m, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Repeat.UpgradeValueBy(1m); // 攻击次数 3 → 4
        }
    }
}