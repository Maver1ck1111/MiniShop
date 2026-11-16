using System.Drawing;
using MiniShop.Tools.Common;

namespace MiniShop.Tools.Pen
{
    public class PenSettings
    {
        public int PixelSize { get; set; } = 1;
        public Pixel Pixel { get; set; } = new();
    }
}
