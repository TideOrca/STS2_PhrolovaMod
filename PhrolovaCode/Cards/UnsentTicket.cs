
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class UnsentTicket : PhrolovaCard
    {
        public UnsentTicket() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("RebirthStacks", 1m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 获得重世
            int stacks = (int)DynamicVars["RebirthStacks"].BaseValue;
            await PowerCmd.Apply<RebirthPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, stacks, Owner.Creature, this, false);

            // 保留手牌（借鉴 Equilibrium）
            await PowerCmd.Apply<RetainHandPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars["RebirthStacks"].UpgradeValueBy(1m); // 1 → 2 层重世
        }
    }
}