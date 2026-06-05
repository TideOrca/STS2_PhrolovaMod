using Phrolova.PhrolovaCode.Powers;
using Phrolova.PhrolovaCode.Relics;

namespace Phrolova.PhrolovaCode.Cards
{
    public abstract class PhrolovaCard : CustomCardModel
    {
        protected PhrolovaCard(int cost, CardType type, CardRarity rarity, TargetType target)
            : base(cost, type, rarity, target) { }

        public override string PortraitPath
        {
            get
            {
                string imageName = BaseLib.Extensions.StringExtensions.RemovePrefix(Id.Entry)
                    .ToLowerInvariant() + ".png";
                string path = "res://Phrolova/images/cards/" + imageName;
                return ResourceLoader.Exists(path) ? path : CardModel.MissingPortraitPath;
            }
        }

        public override string BetaPortraitPath
        {
            get
            {
                string imageName = BaseLib.Extensions.StringExtensions.RemovePrefix(Id.Entry)
                    .ToLowerInvariant() + ".png";
                string path = "res://Phrolova/images/cards/big/" + imageName;
                return ResourceLoader.Exists(path) ? path : PortraitPath;
            }
        }
        
        protected override IEnumerable<IHoverTip> ExtraHoverTips => AutoKeywordTips;

        // 子类可重写此属性来添加额外的提示（如特定能力）
        protected virtual IEnumerable<IHoverTip> AdditionalHoverTips => Enumerable.Empty<IHoverTip>();

        private IEnumerable<IHoverTip> AutoKeywordTips
        {
            get
            {
                var tips = new List<IHoverTip>();
                // 自动检测并添加“重世”关键词提示
                if (CanonicalKeywords.Contains(PhrolovaCode.Keywords.Rebirth))
                    tips.Add(HoverTipFactory.FromKeyword(PhrolovaCode.Keywords.Rebirth));
                    
                // 合并子类自定义的额外提示
                tips.AddRange(AdditionalHoverTips);
                return tips.Distinct();
            }
        }
        
        public static event Action<Creature> RebirthTriggered;
        protected async Task<bool> TryConsumeRebirth()
        {
            if (Owner == null) return false;
            var rebirth = Owner.Creature.Powers.OfType<RebirthPower>().FirstOrDefault();
            if (rebirth == null || rebirth.Amount <= 0) return false;

            // 正常消耗一层重世
            await PowerCmd.ModifyAmount(rebirth, -1, Owner.Creature, null);

            // ★ 同步处理新世界舞曲
            var rhapsody = Owner.Creature.Powers.OfType<NewWorldRhapsodyPower>().FirstOrDefault();
            if (rhapsody != null)
                await rhapsody.OnRebirthTriggered(null); // 此处无上下文，使用 ThrowingContext 后备

            RebirthTriggered?.Invoke(Owner.Creature);
            return true;
        }
    }
}