using System;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Tools;
using MiniShop.Tools.Pen;

namespace MiniShop.Factories
{
    public class ToolsFactory(
        ToolsSettings _settings, 
        IServiceProvider _serviceProvider) : IToolsFactory
    {

        public ITool? CreateTool(ToolMode mode)
        {   
            return mode switch
            {
                ToolMode.Pen => ActivatorUtilities.CreateInstance<Pen>(_serviceProvider, _settings.PenSettings),
                _ => null
            };
        }
    }
}
