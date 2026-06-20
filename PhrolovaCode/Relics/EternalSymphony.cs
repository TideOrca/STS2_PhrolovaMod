#nullable enable
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Relics
{
    [Pool(typeof(PhrolovaRelicPool))]
    public sealed class EternalSymphony : PhrolovaRelic
    {
        public override RelicRarity Rarity => RelicRarity.Starter;
        public override bool SpawnsPets => true;
        public override bool ShowCounter => true;

        private int _playedCount;
        private const int TriggerCount = 3;

        public override int DisplayAmount => _playedCount;

        public override async Task BeforeCombatStart()
        {
            _playedCount = 0;
            InvokeDisplayAmountChanged();

            var tuningList = await PowerCmd.Apply<TuningStatePower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 1, Owner.Creature, null, false);
            var tuning = tuningList?.FirstOrDefault();
            if (tuning != null)
                await tuning.ForceAddNote(TuningStatePower.NoteType.Colorful, 1);

            await PlayerCmd.AddPet<Hecate>(Owner);
            await PowerCmd.Apply<WeakPower>(new ThrowingPlayerChoiceContext(), new[] { Owner.Creature }, 3, Owner.Creature, null, false);
        }

        public override async Task AfterPowerAmountChanged(PlayerChoiceContext choiceContext, PowerModel power, decimal amount, Creature? applier, CardModel? cardSource)
        {
            if (power is TuningStatePower && power.Owner == Owner.Creature)
            {
                _playedCount = 0;
                InvokeDisplayAmountChanged();
            }
        }

        public override async Task AfterCardPlayed(PlayerChoiceContext context, CardPlay cardPlay)
        {
            if (cardPlay.Card.Owner?.Creature != Owner.Creature) return;

            // ★ 只统计攻击、技能、能力牌
            if (cardPlay.Card.Type != CardType.Attack &&
                cardPlay.Card.Type != CardType.Skill &&
                cardPlay.Card.Type != CardType.Power)
                return;

            _playedCount++;
            InvokeDisplayAmountChanged();

            if (_playedCount >= TriggerCount)
            {
                Flash();
                _playedCount = 0;
                InvokeDisplayAmountChanged();
            }
        }

        public override RelicModel? GetUpgradeReplacement() => ModelDb.Relic<EternalSymphonyAncient>();
    }
}