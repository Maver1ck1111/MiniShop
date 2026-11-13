using Avalonia.Media.Imaging;
using MiniShop.Enums;
using MiniShop.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniShop.Factories
{
    public interface IToolsFactory
    {
        public ITool? CreateTool(ToolMode mode, WriteableBitmap bitmap);
    }
}
