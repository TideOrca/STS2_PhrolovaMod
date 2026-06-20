
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Cards
{
    [Pool(typeof(PhrolovaCardPool))]
    public sealed class LifeDeathMovement : PhrolovaCard
    {
        public LifeDeathMovement() : base(1, CardType.Attack, CardRarity.Basic, TargetType.AnyEnemy) { }

        public override IEnumerable<CardKeyword> CanonicalKeywords => new[] { PhrolovaCode.Keywords.Rebirth };

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[]
        {
            new DamageVar(9m, ValueProp.Move),
            new PowerVar<WeakPower>(1m)
        };

        protected override IEnumerable<IHoverTip> ExtraHoverTips => new IHoverTip[]
        {
            HoverTipFactory.FromKeyword(PhrolovaCode.Keywords.Rebirth)
        };

        protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
        {
            // 1. 先检查重世：有则消耗一层，获得红乐
            if (await TryConsumeRebirth())
            {
                var tuning = Owner.Creature.Powers.OfType<TuningStatePower>().FirstOrDefault();
                if (tuning != null)
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Red, 1, choiceContext);
            }

            // 2. 基础效果：伤害 + 虚弱
            await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
                .FromCard(this)
                .Targeting(cardPlay.Target)
                .WithHitFx("vfx/vfx_attack_slash")
                .Execute(choiceContext);

            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), new[] { cardPlay.Target }, DynamicVars["WeakPower"].BaseValue, Owner.Creature, this, false);
        }

        protected override void OnUpgrade()
        {
            DynamicVars.Damage.UpgradeValueBy(2m);
            DynamicVars["WeakPower"].UpgradeValueBy(1m);
        }
    }
}