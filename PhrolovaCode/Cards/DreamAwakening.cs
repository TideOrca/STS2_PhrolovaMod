
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class DreamAwakening : PhrolovaCard
    {
        public DreamAwakening() : base(3, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[]
        {
            CardKeyword.Retain,
            PhrolovaCode.Keywords.Rebirth
        };
        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromPower<DreamMarkPower>()
        };
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DynamicVar("DreamStacks", 19m)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            // 给予所有敌人 19 层迷梦
            var enemies = combatState.GetOpponentsOf(Owner.Creature).ToList();
            foreach (var enemy in enemies)
            {
                await PowerCmd.Apply<DreamMarkPower>(enemy, DynamicVars["DreamStacks"].BaseValue, Owner.Creature, this);
            }

            // 判断重世效果
            bool removeDream = !await TryConsumeRebirth(); // 如果成功消耗重世，则不移除

            foreach (var enemy in enemies)
            {
                var dreamMark = enemy.Powers.OfType<DreamMarkPower>().FirstOrDefault();
                if (dreamMark == null || dreamMark.Amount <= 0) continue;

                int dreamStacks = dreamMark.Amount;
                // 造成迷梦层数的伤害（无视格挡，不受力量虚弱影响）
                await CreatureCmd.Damage(
                    choiceContext,
                    enemy,
                    dreamStacks,
                    ValueProp.Unblockable | ValueProp.Unpowered,
                    Owner.Creature,
                    this
                );

                if (removeDream)
                {
                    await PowerCmd.Remove(dreamMark);
                }
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars["DreamStacks"].UpgradeValueBy(7m);
        }
    }
}