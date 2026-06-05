
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Rebirth : PhrolovaCard
    {
        public Rebirth() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(11m, ValueProp.Move),
            new DynamicVar("RebirthStacks", 2m)
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
                int stacks = (int)DynamicVars["RebirthStacks"].BaseValue;
                await PowerCmd.Apply<RebirthPower>(Owner.Creature, stacks, Owner.Creature, this);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(4m); // 11 → 15
            DynamicVars["RebirthStacks"].UpgradeValueBy(1m);
        }
    }
}