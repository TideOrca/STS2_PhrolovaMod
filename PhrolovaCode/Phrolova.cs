#nullable enable

using Phrolova.PhrolovaCode.Cards;
using Phrolova.PhrolovaCode.Pools;
using Phrolova.PhrolovaCode.Relics;

namespace Phrolova
{
    public sealed class Phrolova : PlaceholderCharacterModel
    {
        public override string PlaceholderID => "necrobinder";
        
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
            "res://Phrolova/scenes/phrolova_battle.tscn";
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
        public override Color NameColor => new Color("#7A2F8F");
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

        // 音效
        public override string CharacterSelectSfx => string.Empty;
        public override string CharacterTransitionSfx => "event:/sfx/ui/phrolova_transition";
    }
}