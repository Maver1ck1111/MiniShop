using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.ObservableStateServices;
using MiniShop.Tools;
using System;

namespace MiniShop.Views;

public partial class ToolsBar : UserControl
{   
    private bool _toolSelected = false;
    private IToolStateService _servcice;
    
    public ToolsBar()
    {
        InitializeComponent();
        _servcice = App.ServiceProvider.GetRequiredService<IToolStateService>();
    }

    private void OnToolSelected(object? sender, RoutedEventArgs e)
    {
        Button? button = null;
        
        if (sender is not Button) return;
        
        button = sender as Button;
        
        if (Enum.TryParse<ToolMode>(button.Name, ignoreCase: true, out var result))
        {
            if (_toolSelected) 
            {
                _toolSelected = false;
                _servcice.SetTool(result);
                return;
            }

            _toolSelected = true;
            _servcice.SetTool(result);
        } 
    }
}