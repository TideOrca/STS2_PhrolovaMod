
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class NightmareStrike : PhrolovaCard
    {
        public NightmareStrike() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(7m, ValueProp.Move)
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };
        
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var attack = await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            int totalDamage = (int)attack.Results.SelectMany(r => r).Sum(r => r.UnblockedDamage);
            if (totalDamage > 0)
            {
                await PowerCmd.Apply<DreamMarkPower>(new ThrowingPlayerChoiceContext(), new[] { cardPlay.Target }, totalDamage, Owner.Creature, this, false);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m); // 7 → 9
        }
    }
}