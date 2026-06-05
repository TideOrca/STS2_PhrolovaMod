
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class CurtainCall : PhrolovaCard
    {
        public CurtainCall() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => IsUpgraded 
            ? new[] { CardKeyword.Retain } 
            : Enumerable.Empty<CardKeyword>();

        protected override bool IsPlayable
        {
            get
            {
                if (Owner == null) return false;
                return Owner.Creature.Powers.OfType<PerformingPower>().Any();
            }
        }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(0, ValueProp.Move) // 动态更新为累计伤害
        };

        // 卡牌进入手牌时更新伤害预览
        public override async Task AfterCardDrawn(PlayerChoiceContext choiceContext, CardModel card, bool fromHandDraw)
        {
            UpdateDamageFromPerformance();
            await Task.CompletedTask;
        }

        // 卡牌进入战斗时也更新
        public override async Task AfterCardEnteredCombat(CardModel card)
        {
            UpdateDamageFromPerformance();
            await Task.CompletedTask;
        }

        private void UpdateDamageFromPerformance()
        {
            if (Owner == null) return;
            var performing = Owner.Creature.Powers.OfType<PerformingPower>().FirstOrDefault();
            if (performing != null)
                DynamicVars.Damage.BaseValue = performing.TotalDamageDealt;
            else
                DynamicVars.Damage.BaseValue = 0;
        }

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            var performing = Owner.Creature.Powers.OfType<PerformingPower>().FirstOrDefault();
            if (performing == null) return;

            // 动态变量已自动更新，直接使用
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);
            
            foreach (var p in Owner.Creature.Powers.OfType<RedNotePower>().ToList()) await PowerCmd.Remove(p);
            foreach (var p in Owner.Creature.Powers.OfType<BlueNotePower>().ToList()) await PowerCmd.Remove(p);
            foreach (var p in Owner.Creature.Powers.OfType<ColorfulNotePower>().ToList()) await PowerCmd.Remove(p);

            // 移除演奏，进入定音
            await PowerCmd.Remove(performing);
            await PowerCmd.Apply<TuningStatePower>(Owner.Creature, 1, Owner.Creature, null);
        }
        protected override void OnUpgrade()
        {
            AddKeyword(CardKeyword.Retain); // 升级时强制添加保留
        }
    }
}