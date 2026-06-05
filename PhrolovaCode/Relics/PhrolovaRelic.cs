using BaseLib.Abstracts;
using Godot;

namespace Phrolova.Relics
{
    public abstract class PhrolovaRelic : CustomRelicModel
    {
        protected PhrolovaRelic() : base(true) { }

        public override string PackedIconPath
        {
            get
            {
                string imageName = BaseLib.Extensions.StringExtensions.RemovePrefix(Id.Entry)
                    .ToLowerInvariant() + ".png";
                string path = "res://Phrolova/images/relics/" + imageName;
                return ResourceLoader.Exists(path) ? path : "res://Phrolova/images/relics/relic.png";
            }
        }

        protected override string PackedIconOutlinePath
        {
            get
            {
                string imageName = BaseLib.Extensions.StringExtensions.RemovePrefix(Id.Entry)
                    .ToLowerInvariant() + "_outline.png";
                string path = "res://Phrolova/images/relics/" + imageName;
                return ResourceLoader.Exists(path) ? path : PackedIconPath;
            }
        }

        protected override string BigIconPath
        {
            get
            {
                string imageName = BaseLib.Extensions.StringExtensions.RemovePrefix(Id.Entry)
                    .ToLowerInvariant() + ".png";
                string path = "res://Phrolova/images/relics/big/" + imageName;
                return ResourceLoader.Exists(path) ? path : PackedIconPath;
            }
        }
    }
}