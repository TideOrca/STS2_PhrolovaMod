
namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class SongForYou : PhrolovaCard
    {
        public SongForYou() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.AnyAlly) { }

        // 仅多人模式可用
        public override CardMultiplayerConstraint MultiplayerConstraint => CardMultiplayerConstraint.MultiplayerOnly;

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("StrengthGiven", 0m),
            new DynamicVar("DexterityGiven", 0m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (Owner == null || cardPlay.Target?.Player == null) return;

            int str = Owner.Creature.GetPowerAmount<StrengthPower>();
            int dex = Owner.Creature.GetPowerAmount<DexterityPower>();
            if (str + dex == 0) return;

            DynamicVars["StrengthGiven"].BaseValue = str;
            DynamicVars["DexterityGiven"].BaseValue = dex;

            var targetCreature = cardPlay.Target;

            if (str > 0)
                await PowerCmd.Apply<StrengthPower>(targetCreature, str, Owner.Creature, this);
            if (dex > 0)
                await PowerCmd.Apply<DexterityPower>(targetCreature, dex, Owner.Creature, this);
        }

        protected override void OnUpgrade()
        {
            EnergyCost.UpgradeBy(-1);
        }
    }
}