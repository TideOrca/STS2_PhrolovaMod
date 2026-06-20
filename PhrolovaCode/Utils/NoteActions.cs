
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Utils
{
 public static class NoteActions
    {
        public static async Task<bool> TryConsumeOneNote(TuningStatePower tuning, PlayerChoiceContext? context = null)
        {
            if (tuning?.Owner == null || tuning.NoteOrder.Count == 0) return false;
            
            var owner = tuning.Owner; 
            int targetIndex = -1; 
            TuningStatePower.NoteType targetType = default;

            // 1. 优先寻找最早的非彩乐
            for (int i = 0; i < tuning.NoteOrder.Count; i++) 
            { 
                if (tuning.NoteOrder[i] != TuningStatePower.NoteType.Colorful) 
                { 
                    // 确保该能力实例真的存在
                    bool exists = tuning.NoteOrder[i] == TuningStatePower.NoteType.Red 
                        ? owner.Powers.OfType<RedNotePower>().Any() 
                        : owner.Powers.OfType<BlueNotePower>().Any(); 
                    if (exists) 
                    { 
                        targetIndex = i; 
                        targetType = tuning.NoteOrder[i]; 
                        break; 
                    } 
                    // 否则这个列表条目是僵尸，直接清除
                    tuning.NoteOrder.RemoveAt(i); 
                    i--; 
                } 
            }

            // 2. 如果没有非彩乐，则寻找最早的彩乐
            if (targetIndex == -1) 
            { 
                for (int i = 0; i < tuning.NoteOrder.Count; i++) 
                { 
                    if (tuning.NoteOrder[i] == TuningStatePower.NoteType.Colorful) 
                    {
                        if (owner.Powers.OfType<ColorfulNotePower>().Any())
                        {
                            targetIndex = i;
                            targetType = TuningStatePower.NoteType.Colorful;
                            break;
                        }
                        tuning.NoteOrder.RemoveAt(i);
                        i--;
                    }
                }
            }

            // 3. 没有任何有效的乐声可消耗
            if (targetIndex == -1) return false;

            // 4. 减1层对应能力（Counter模式），层数为0时自动移除
            tuning.NoteOrder.RemoveAt(targetIndex);
            var ctx = context ?? new ThrowingPlayerChoiceContext();
            if (targetType == TuningStatePower.NoteType.Red)
            {
                var p = owner.Powers.OfType<RedNotePower>().FirstOrDefault();
                if (p != null && p.Amount > 0)
                    await PowerCmd.ModifyAmount(ctx, p, -1, owner, null, false);
            }
            else if (targetType == TuningStatePower.NoteType.Blue)
            {
                var p = owner.Powers.OfType<BlueNotePower>().FirstOrDefault();
                if (p != null && p.Amount > 0)
                    await PowerCmd.ModifyAmount(ctx, p, -1, owner, null, false);
            }
            else if (targetType == TuningStatePower.NoteType.Colorful)
            {
                var p = owner.Powers.OfType<ColorfulNotePower>().FirstOrDefault();
                if (p != null && p.Amount > 0)
                    await PowerCmd.ModifyAmount(ctx, p, -1, owner, null, false);
            }
            return true;
}

        // 将最早的一个非彩乐转换为彩乐
        public static async Task ConvertToColorful(TuningStatePower tuning, PlayerChoiceContext? context = null)
        {
            if (tuning?.Owner == null) return;
            var owner = tuning.Owner;
            var list = tuning.NoteOrder;

            int targetIndex = -1;
            TuningStatePower.NoteType targetType = default;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == TuningStatePower.NoteType.Colorful) continue;
                bool exists = list[i] == TuningStatePower.NoteType.Red
                    ? owner.Powers.OfType<RedNotePower>().Any()
                    : owner.Powers.OfType<BlueNotePower>().Any();
                if (exists) { targetIndex = i; targetType = list[i]; break; }
                else { list.RemoveAt(i); i--; }
            }
            if (targetIndex == -1) return;

            var ctx2 = context ?? new ThrowingPlayerChoiceContext();
            if (targetType == TuningStatePower.NoteType.Red)
            {
                var p = owner.Powers.OfType<RedNotePower>().FirstOrDefault();
                if (p != null && p.Amount > 0)
                    await PowerCmd.ModifyAmount(ctx2, p, -1, owner, null, false);
            }
            else if (targetType == TuningStatePower.NoteType.Blue)
            {
                var p = owner.Powers.OfType<BlueNotePower>().FirstOrDefault();
                if (p != null && p.Amount > 0)
                    await PowerCmd.ModifyAmount(ctx2, p, -1, owner, null, false);
            }

            list.RemoveAt(targetIndex);
            list.Add(TuningStatePower.NoteType.Colorful);
            await PowerCmd.Apply<ColorfulNotePower>(ctx2, new[] { owner }, 1, owner, null, false);
        }

        // 将所有非彩乐转换为彩乐
        public static async Task ConvertAllToColorful(TuningStatePower tuning, PlayerChoiceContext? context = null)
        {
            if (tuning?.Owner == null) return;
            var owner = tuning.Owner;
            var list = tuning.NoteOrder;

            var redPower = owner.Powers.OfType<RedNotePower>().FirstOrDefault();
            var bluePower = owner.Powers.OfType<BlueNotePower>().FirstOrDefault();
            int redCount = (int)(redPower?.Amount ?? 0);
            int blueCount = (int)(bluePower?.Amount ?? 0);
            int totalConverted = redCount + blueCount;
            if (totalConverted == 0) return;

            var ctx3 = context ?? new ThrowingPlayerChoiceContext();
            if (redPower != null && redCount > 0)
                await PowerCmd.ModifyAmount(ctx3, redPower, -redCount, owner, null, false);
            if (bluePower != null && blueCount > 0)
                await PowerCmd.ModifyAmount(ctx3, bluePower, -blueCount, owner, null, false);
            list.RemoveAll(n => n != TuningStatePower.NoteType.Colorful);

            for (int i = 0; i < totalConverted; i++)
            {
                await PowerCmd.Apply<ColorfulNotePower>(ctx3, new[] { owner }, 1, owner, null, false);
                list.Add(TuningStatePower.NoteType.Colorful);
            }
        }
    }
}