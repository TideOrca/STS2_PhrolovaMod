
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class SwingBat : PhrolovaCard
    {
        public SwingBat() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(10m, ValueProp.Move),
            new CardsVar(2)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            if (await TryConsumeRebirth())
            {
                await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m); // 10 → 13
        }
    }
}