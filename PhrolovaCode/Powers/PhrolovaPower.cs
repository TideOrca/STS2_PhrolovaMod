
namespace Phrolova.Powers
{
    public abstract class PhrolovaPower : CustomPowerModel
    {
        public override string CustomPackedIconPath
        {
            get
            {
                string imageName = BaseLib.Extensions.StringExtensions.RemovePrefix(Id.Entry)
                    .ToLowerInvariant() + ".png";
                string path = "res://Phrolova/images/powers/" + imageName;
                return ResourceLoader.Exists(path) ? path : "res://images/powers/missing_power.png";
            }
        }

        public override string CustomBigIconPath
        {
            get
            {
                string imageName = BaseLib.Extensions.StringExtensions.RemovePrefix(Id.Entry)
                    .ToLowerInvariant() + ".png";
                string path = "res://Phrolova/images/powers/big/" + imageName;
                return ResourceLoader.Exists(path) ? path : CustomPackedIconPath;
            }
        }
    }
}