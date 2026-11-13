using Avalonia.Media.Imaging;
using MiniShop.Enums;
using MiniShop.Tools;
using MiniShop.Tools.ToolsSettings;

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
