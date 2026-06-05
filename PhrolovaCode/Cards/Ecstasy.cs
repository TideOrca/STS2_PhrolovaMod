
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Ecstasy : PhrolovaCard
    {
        public Ecstasy() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("DreamStacks", 4m)
        };
        
        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };
        
        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 抽1张牌
            await CardPileCmd.Draw(choiceContext, 1, Owner);

            // 给予有迷梦的敌人额外6层迷梦
            var combatState = Owner?.Creature?.CombatState;
            if (combatState != null)
            {
                var enemies = combatState.GetOpponentsOf(Owner.Creature);
                foreach (var enemy in enemies)
                {
                    if (enemy.Powers.OfType<DreamMarkPower>().Any())
                    {
                        await PowerCmd.Apply<DreamMarkPower>(enemy, DynamicVars["DreamStacks"].BaseValue, Owner.Creature, this);
                    }
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["DreamStacks"].UpgradeValueBy(2m);
        }
    }
}