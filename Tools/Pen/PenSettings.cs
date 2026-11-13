namespace MiniShop.Tools.Pen
{
    public class PenSettings
    {
        public int PixelSize { get; set; } = 1;
        public (byte, byte, byte) Color { get; set; } = (255, 255, 255);
        public byte Opacity { get; set; } = 100;
    }
}
