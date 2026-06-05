
using HarmonyLib;
using BaseLib.Patches.Content;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Nodes.Screens.CharacterSelect;
using Phrolova.PhrolovaCode.Cards;
using Phrolova.PhrolovaCode.Extensions;
using Phrolova.PhrolovaCode.Powers;

namespace Phrolova.PhrolovaCode
{
    public static class ModInitializer
    {
        // 自定义枚举放在类级别
        [CustomEnum]
        public static CardTag HecateAttack;

        public static void Initialize()
        {
            Log.Info("Phrolova mod initialized with BaseLib!");

            // 创建 Harmony 实例并应用所有补丁
            var harmony = new Harmony("author.phrolova");
            harmony.PatchAll();
        }
    }

    // 古老牙齿补丁：把弗洛洛的两张初始卡加进映射表
    [HarmonyPatch(typeof(ArchaicTooth), "TranscendenceUpgrades", MethodType.Getter)]
    public static class ArchaicToothTranscendencePatch
    {
        static void Postfix(ref Dictionary<ModelId, CardModel> __result)
        {
            __result[ModelDb.Card<LifeDeathMovement>().Id] = ModelDb.Card<DeathLifeMovement>();
            __result[ModelDb.Card<FleetingDream>().Id] = ModelDb.Card<EternalDream>();
        }
    }
    
    [HarmonyPatch(typeof(WeakPower), nameof(WeakPower.ModifyDamageMultiplicative))]
    public static class WeakPowerIgnorePatch
    {
        static bool Prefix(Creature target, ref decimal amount, ValueProp props, Creature dealer, CardModel cardSource, ref decimal __result)
        {
            // 如果攻击者拥有“红醋栗果饼”能力，则直接返回 1m，即虚弱无效
            if (dealer != null && dealer.HasPower<IgnoreWeaknessPower>())
            {
                __result = 1m;
                return false; // 跳过原方法
            }
            return true; // 否则正常执行原方法
        }
    }
    
    [HarmonyPatch(typeof(NCharacterSelectScreen), "SelectCharacter")]
    public static class PhrolovaSelectSound
    {
        private static readonly System.Random _sfxRandom = new System.Random();

        private static void Prefix(NCharacterSelectButton charSelectButton, CharacterModel characterModel)
        {
            if (characterModel is Phrolova)
            {
                int index = _sfxRandom.Next(1, 4); // 随机 1~3
                PhrolovaAudio.Play($"res://Phrolova/audio/phrolova_select{index}");
            }
        }
    }
    // 转场音效（拦截 SfxCmd.Play 中对 transition 的调用）
    [HarmonyPatch(typeof(SfxCmd), nameof(SfxCmd.Play), new Type[] { typeof(string), typeof(float) })]
    public static class PhrolovaTransitionSound
    {
        static bool Prefix(string sfx, float volume = 1f)
        {
            // 如果正在播放 phrolova 的转场音效（虚拟路径），替换为 MP3
            if (sfx == "event:/sfx/ui/phrolova_transition")
            {
                PhrolovaAudio.Play("res://Phrolova/audio/phrolova_transition");
                return false; // 跳过 FMOD 调用
            }
            return true;
        }
    }
}