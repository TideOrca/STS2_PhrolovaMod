
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class NoMoreWaiting : PhrolovaCard
    {
        public NoMoreWaiting() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };
        
        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(10m, ValueProp.Move),
            new PowerVar<WeakPower>(3m),
            new DynamicVar("DreamStacks", 11m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this, false);

            if (await TryConsumeRebirth())
            {
                await PowerCmd.Apply<DreamMarkPower>(new ThrowingPlayerChoiceContext(), new[] { cardPlay.Target }, DynamicVars["DreamStacks"].BaseValue, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m);     // 10 → 13
            DynamicVars["DreamStacks"].UpgradeValueBy(3m); // 11 → 14
        }
    }
}