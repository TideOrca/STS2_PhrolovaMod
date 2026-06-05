
using MegaCrit.Sts2.Core.Entities.Players;
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(TokenCardPool))]
    public sealed class SecretOfUnderworld : PhrolovaCard
    {
        public SecretOfUnderworld() : base(0, CardType.Skill, CardRarity.Token, TargetType.Self) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust };
        public override bool CanBeGeneratedInCombat => true;

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("ColorfulNotes", 1m)
        };

        // 官方风格创建方法
        public static IEnumerable<SecretOfUnderworld> Create(Player owner, int amount, CombatState combatState)
        {
            var list = new List<SecretOfUnderworld>();
            for (int i = 0; i < amount; i++)
            {
                list.Add(combatState.CreateCard<SecretOfUnderworld>(owner));
            }
            return list;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            int count = (int)DynamicVars["ColorfulNotes"].BaseValue;
            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
            {
                for (int i = 0; i < count; i++)
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 1, choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["ColorfulNotes"].UpgradeValueBy(1m);
        }
    }
}