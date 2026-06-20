#nullable enable

using MegaCrit.Sts2.Core.Animation;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using Phrolova.PhrolovaCode.Cards;
using Phrolova.PhrolovaCode.Pools;
using Phrolova.PhrolovaCode.Relics;

namespace Phrolova
{
    public sealed class Phrolova : PlaceholderCharacterModel
    {
        public override string CustomIconTexturePath =>
            "res://Phrolova/images/characters/icon_phrolova.png";
        public override string CustomIconOutlineTexturePath =>
            "res://Phrolova/images/characters/icon_phrolova_outline.png";
        public override string CustomCharacterSelectIconPath =>
            "res://Phrolova/images/characters/char_select_phrolova.png";
        public override string CustomCharacterSelectLockedIconPath =>
            "res://Phrolova/images/characters/char_select_phrolova_locked.png";
        public override string CustomMapMarkerPath =>
            "res://Phrolova/images/ui/map_marker_phrolova.png";
        public override string CustomCharacterSelectBg => 
            "res://Phrolova/scenes/char_select_bg_phrolova.tscn";  
        public override string CustomIconPath => 
            "res://Phrolova/scenes/phrolova_icon.tscn";
        
        public override string CustomArmPointingTexturePath =>
            "res://Phrolova/images/ui/hands/multiplayer_hand_phrolova_point.png";
        public override string CustomArmRockTexturePath =>
            "res://Phrolova/images/ui/hands/multiplayer_hand_phrolova_rock.png";
        public override string CustomArmPaperTexturePath =>
            "res://Phrolova/images/ui/hands/multiplayer_hand_phrolova_paper.png";
        public override string CustomArmScissorsTexturePath =>
            "res://Phrolova/images/ui/hands/multiplayer_hand_phrolova_scissors.png";
        
        // 战斗场景路径
        public override string CustomVisualPath =>
            "res://Phrolova/scenes/phrolova_spine.tscn";
        // 火堆休息场景
        public override string CustomRestSiteAnimPath =>
            "res://Phrolova/scenes/phrolova_rest_site.tscn"; 
        // 商店站立场景
        public override string CustomMerchantAnimPath =>
            "res://Phrolova/scenes/phrolova_merchant.tscn";
        
        public override string CustomEnergyCounterPath =>
            "res://Phrolova/scenes/phrolova_energy_counter.tscn";
        
        // 基础属性
        public override CharacterGender Gender => CharacterGender.Feminine;
        public override Color NameColor => new Color("#BB0000");
        public override int StartingHp => 70;
        public override int StartingGold => 99;

        // 池子
        public override CardPoolModel CardPool => ModelDb.CardPool<PhrolovaCardPool>();
        public override RelicPoolModel RelicPool => ModelDb.RelicPool<PhrolovaRelicPool>();
        public override PotionPoolModel PotionPool => ModelDb.PotionPool<PhrolovaPotionPool>();

        // 初始卡组
        public override IEnumerable<CardModel> StartingDeck => new CardModel[]
        {
            ModelDb.Card<StrikePhrolova>(),
            ModelDb.Card<StrikePhrolova>(),
            ModelDb.Card<StrikePhrolova>(),
            ModelDb.Card<StrikePhrolova>(),
            ModelDb.Card<DefendPhrolova>(),
            ModelDb.Card<DefendPhrolova>(),
            ModelDb.Card<DefendPhrolova>(),
            ModelDb.Card<DefendPhrolova>(),
            ModelDb.Card<LifeDeathMovement>(),
            ModelDb.Card<FleetingDream>()
        };

        public override IReadOnlyList<RelicModel> StartingRelics => new[]
        {
            ModelDb.Relic<EternalSymphony>()
        };

        public override float AttackAnimDelay => 0.15f;
        public override float CastAnimDelay => 0.25f;

        // 自定义动画映射：将引擎事件绑定到 Spine 骨骼中的实际动画名
        // 如果此处编译报错 "no suitable method found to override"，
        public override CreatureAnimator SetupCustomAnimationStates(MegaSprite controller)
        {
            AnimState idle = new AnimState("idle", true);
            AnimState cast = new AnimState("fail", false);
            AnimState attack = new AnimState("skill", false);
            AnimState hurt = new AnimState("success", false);
            AnimState die = new AnimState("success", false);
            AnimState deadLoop = new AnimState("success_loop", true);

            idle.AddBranch("Hit", hurt, null);
            cast.NextState = idle; cast.AddBranch("Hit", hurt, null);
            attack.NextState = idle; attack.AddBranch("Hit", hurt, null);
            hurt.NextState = idle; hurt.AddBranch("Hit", hurt, null);
            die.NextState = deadLoop;

            CreatureAnimator animator = new CreatureAnimator(idle, controller);
            animator.AddAnyState("Attack", attack, null);
            animator.AddAnyState("Cast", cast, null);
            animator.AddAnyState("Dead", die, null);
            return animator;
        }

        // 音效
        public override string CharacterSelectSfx => string.Empty;
        public override string CharacterTransitionSfx => "event:/sfx/ui/phrolova_transition";
    }
}