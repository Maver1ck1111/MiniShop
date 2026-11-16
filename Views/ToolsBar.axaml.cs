using Avalonia.Controls;
using Avalonia.Interactivity;
using MiniShop.Tools;
using System;

namespace MiniShop.Views;

public partial class ToolsBar : UserControl
{
    public event EventHandler<ToolMode>? ToolSelected;

    private bool _toolSelected = false;
    public ToolsBar()
    {
        InitializeComponent();
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
                ToolSelected?.Invoke(this, ToolMode.None);
                return;
            }

            _toolSelected = true;
            ToolSelected?.Invoke(this, result);
        } 
    }
}