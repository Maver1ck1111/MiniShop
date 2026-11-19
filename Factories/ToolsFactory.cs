using System;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.ObservableStateServices;
using MiniShop.Tools;
using MiniShop.Tools.Pen;

namespace MiniShop.Factories
{
    public class ToolsFactory(
        ToolsSettings _settings, 
        IServiceProvider _serviceProvider, 
        ILayersStateService _layersService) : IToolsFactory
    {

        public ITool? CreateTool(ToolMode mode)
        {
            if (_layersService.SelectedLayer is null) return null;
            
            return mode switch
            {
                ToolMode.Pen => ActivatorUtilities.CreateInstance<Pen>(_serviceProvider, _settings.PenSettings, _layersService.SelectedLayer),
                _ => null
            };
        }
    }
}
