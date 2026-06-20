using Phrolova.PhrolovaCode.Extensions;
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(TokenCardPool))]
    public sealed class FinalMovement : PhrolovaCard
    {
        private static readonly Random _random = new Random();
        
        public FinalMovement() : base(0, CardType.Attack, CardRarity.Token, TargetType.AllEnemies) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { CardKeyword.Exhaust, CardKeyword.Retain };

        // 动态变量：伤害、能量、抽牌
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(20m, ValueProp.Move),
            new EnergyVar(3),
            new CardsVar(3)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 播放谱曲终末音效
            int index = _random.Next(1, 8); // 1~7 随机
            PhrolovaAudio.Play($"res://Phrolova/audio/final_movement{index}");
            
            var combatState = Owner?.Creature?.CombatState;
            if (combatState == null) return;

            // 使用动态变量获取实际伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .TargetingAllOpponents(combatState)
                .Execute(choiceContext);

            // 使用动态变量回复能量
            await PlayerCmd.GainEnergy(DynamicVars.Energy.BaseValue, Owner);
            // 使用动态变量抽牌
            await CardPileCmd.Draw(choiceContext, DynamicVars.Cards.BaseValue, Owner);

            // 读取乐声数量，移除定音，进入演奏
            int red = 0, blue = 0, colorful = 0;
            var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
            if (tuning != null)
            {
                red = Owner.Creature.Powers.OfType<RedNotePower>().Count();
                blue = Owner.Creature.Powers.OfType<BlueNotePower>().Count();
                colorful = Owner.Creature.Powers.OfType<ColorfulNotePower>().Count();

                await PowerCmd.Remove(tuning);
                //foreach (var p in Owner.Creature.Powers.OfType<RedNotePower>().ToList()) await PowerCmd.Remove(p);
                //foreach (var p in Owner.Creature.Powers.OfType<BlueNotePower>().ToList()) await PowerCmd.Remove(p);
                //foreach (var p in Owner.Creature.Powers.OfType<ColorfulNotePower>().ToList()) await PowerCmd.Remove(p);
            }

            var performingList = await PowerCmd.Apply<PerformingPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, null, false);
            var performing = performingList?.FirstOrDefault();
            if (performing != null)
                await performing.ApplyNoteEffects(red, blue, colorful);
        }
        
        protected override void OnUpgrade()
        {
            DynamicVars.Cards.UpgradeValueBy(2m);
        }
    }
}