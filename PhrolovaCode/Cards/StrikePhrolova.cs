
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class StrikePhrolova : PhrolovaCard
    {
        public StrikePhrolova() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }

        protected override HashSet<CardTag> CanonicalTags => new() { CardTag.Strike };

        protected override IEnumerable<DynamicVar> CanonicalVars => new[] { new DamageVar(6m, ValueProp.Move) };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
        }

        protected override void OnUpgrade() => DynamicVars.Damage.UpgradeValueBy(3m);
    }
}