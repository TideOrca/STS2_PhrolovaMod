
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class MusicStrike : PhrolovaCard
    {
        public MusicStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(9m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 造成伤害
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            int targetCount;
            var performing = Owner.Creature.Powers.OfType<PerformingPower>().FirstOrDefault();
            if (performing != null)
            {
                // 演奏状态下：抽牌至手牌大于6张
                targetCount = 6; // 手牌目标数 = 6 + 1
            }
            else
            {
                // 非演奏状态：按乐声数量 + 1
                var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
                int totalNotes = tuning != null ? tuning.GetCurrentTotal() : 0;
                targetCount = totalNotes;
            }

            int handSize = PileType.Hand.GetPile(Owner)?.Cards.Count ?? 0;
            int drawCount = System.Math.Max(0, targetCount - handSize);

            if (drawCount > 0)
                await CardPileCmd.Draw(choiceContext, drawCount, Owner);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(3m); // 9 → 12
        }
    }
}