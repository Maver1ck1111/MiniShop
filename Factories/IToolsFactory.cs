using MiniShop.Tools;

namespace MiniShop.Factories
{
    public interface IToolsFactory
    {
        public ITool? CreateTool(ToolMode mode);
    }
}
