
namespace Phrolova.PhrolovaCode.Pools
{
    public sealed class PhrolovaCardPool : CustomCardPoolModel
    {
        public override string Title => "phrolova";

        // 能量图标路径（指向你的 PNG 图片）
        public override string BigEnergyIconPath =>
            "res://Phrolova/images/ui/big_energy.png";
        public override string TextEnergyIconPath =>
            "res://Phrolova/images/ui/text_energy.png";
        
        public override float H => 0f;
        public override float S => 0.90f;
        public override float V => 0.45f; 

        public override Color DeckEntryCardColor => new Color("580000"); 
        public override Color EnergyOutlineColor => new Color("#580000"); 

        public override bool IsColorless => false;
    }
}