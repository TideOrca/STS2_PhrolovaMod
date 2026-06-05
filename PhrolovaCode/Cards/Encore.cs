
using MegaCrit.Sts2.Core.Combat.History.Entries;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class Encore : PhrolovaCard
    {
        public Encore() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(6m, ValueProp.Move)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            if (cardPlay.Target == null) return;

            // 统计本回合此前自己对该敌人造成的攻击次数
            int hitCount = CombatManager.Instance.History.Entries
                .OfType<DamageReceivedEntry>()
                .Count(e => e.Receiver == cardPlay.Target && 
                            e.Dealer == Owner.Creature && 
                            e.Result.Props.IsPoweredAttack() && 
                            e.HappenedThisTurn(CombatState));

            if (hitCount > 0)
            {
                await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                    .WithHitCount(hitCount)
                    .FromCard(this)
                    .Targeting(cardPlay.Target)
                    .WithHitFx("vfx/vfx_attack_slash")
                    .Execute(choiceContext);
            }
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}