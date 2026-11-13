using Avalonia.Media.Imaging;
using MiniShop.Tools;
using MiniShop.Tools.Pen;

namespace MiniShop.Factories
{
    public class ToolsFactory(ToolsSettings _settings) : IToolsFactory
    {
        public ITool? CreateTool(ToolMode mode, WriteableBitmap bitmap)
        {
            return mode switch
            {
                ToolMode.Pen => new Pen(_settings.PenSettings, bitmap),
                _ => null
            };
        }
    }
}
