
namespace Phrolova.PhrolovaCode.Powers
{
    public sealed class NewWorldRhapsodyPower : PhrolovaPower
    {
        public override PowerType Type => PowerType.Buff;
        public override PowerStackType StackType => PowerStackType.Single;
        public override bool IsInstanced => false;

        private int _rebirthCount;
        public override int DisplayAmount => 3 - _rebirthCount % 3;

        protected override IEnumerable<DynamicVar> CanonicalVars => new[]
        {
            new DynamicVar("Remaining", 3m)
        };

        /// <summary>
        /// 由 TryConsumeRebirth 调用，同步处理重世计数。
        /// context 可选，用于联机安全；无上下文时用 ThrowingContext 后备。
        /// </summary>
        internal async Task OnRebirthTriggered(PlayerChoiceContext? context = null)
        {
            if (Owner == null) return;

            _rebirthCount++;
            int remaining = 3 - _rebirthCount % 3;
            DynamicVars["Remaining"].BaseValue = remaining;
            InvokeDisplayAmountChanged();

            if (_rebirthCount >= 3)
            {
                _rebirthCount = 0;
                var tuning = Owner.Powers.OfType<TuningStatePower>().FirstOrDefault();
                if (tuning != null)
                    await tuning.ForceAddNote(TuningStatePower.NoteType.Red, 1, context);
                Flash();
            }
        }
    }
}