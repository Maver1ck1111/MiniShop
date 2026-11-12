using Avalonia.Media.Imaging;
using MiniShop.Enums;
using MiniShop.Tools;

namespace MiniShop.Factories
{
    public class ToolsFactory()
    {
        public ITool? CreateTool(ToolMode mode, WriteableBitmap bitmap)
        {
            return mode switch
            {
                ToolMode.Pen => new Pen(bitmap),
                _ => null
            };
        }
    }
}
