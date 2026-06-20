
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode.Utils
{
 public static class NoteActions
    {
        public static async Task<bool> TryConsumeOneNote(TuningStatePower tuning) 
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

            // 4. 移除对应的能力实例
            tuning.NoteOrder.RemoveAt(targetIndex);
            if (targetType == TuningStatePower.NoteType.Red)
            {
                var p = owner.Powers.OfType<RedNotePower>().FirstOrDefault();
                if (p != null) await PowerCmd.Remove(p);
            }
            else if (targetType == TuningStatePower.NoteType.Blue)
            {
                var p = owner.Powers.OfType<BlueNotePower>().FirstOrDefault();
                if (p != null) await PowerCmd.Remove(p);
            }
            else if (targetType == TuningStatePower.NoteType.Colorful)
            {
                var p = owner.Powers.OfType<ColorfulNotePower>().FirstOrDefault();
                if (p != null) await PowerCmd.Remove(p);
            }
            return true;
}

        // 将最早的一个非彩乐转换为彩乐
        public static async Task ConvertToColorful(TuningStatePower tuning)
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

            if (targetType == TuningStatePower.NoteType.Red)
            {
                var p = owner.Powers.OfType<RedNotePower>().FirstOrDefault();
                if (p != null) await PowerCmd.Remove(p);
            }
            else if (targetType == TuningStatePower.NoteType.Blue)
            {
                var p = owner.Powers.OfType<BlueNotePower>().FirstOrDefault();
                if (p != null) await PowerCmd.Remove(p);
            }

            list.RemoveAt(targetIndex);
            list.Add(TuningStatePower.NoteType.Colorful);
            await PowerCmd.Apply<ColorfulNotePower>(new ThrowingPlayerChoiceContext(), new[] { owner }, 1, owner, null, false);
        }

        // 将所有非彩乐转换为彩乐
        public static async Task ConvertAllToColorful(TuningStatePower tuning)
        {
            if (tuning?.Owner == null) return;
            var owner = tuning.Owner;
            var list = tuning.NoteOrder;

            int redCount = owner.Powers.OfType<RedNotePower>().Count();
            int blueCount = owner.Powers.OfType<BlueNotePower>().Count();
            int totalConverted = redCount + blueCount;
            if (totalConverted == 0) return;

            foreach (var p in owner.Powers.OfType<RedNotePower>().ToList()) await PowerCmd.Remove(p);
            foreach (var p in owner.Powers.OfType<BlueNotePower>().ToList()) await PowerCmd.Remove(p);
            list.RemoveAll(n => n != TuningStatePower.NoteType.Colorful);

            for (int i = 0; i < totalConverted; i++)
            {
                await PowerCmd.Apply<ColorfulNotePower>(new ThrowingPlayerChoiceContext(), new[] { owner }, 1, owner, null, false);
                list.Add(TuningStatePower.NoteType.Colorful);
            }
        }
    }
}