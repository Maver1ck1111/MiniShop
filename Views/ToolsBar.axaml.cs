using Avalonia.Controls;
using Avalonia.Interactivity;
using MiniShop.Enums;
using System;

namespace MiniShop;

public partial class ToolsBar : UserControl
{
    public event EventHandler<ToolMode>? ToolSelected;

    private bool _penSelected = false;
    public ToolsBar()
    {
        InitializeComponent();
    }

    private void OnPenSelected(object? sender, RoutedEventArgs e)
    {
        if (_penSelected) 
        {
            _penSelected = false;
            ToolSelected?.Invoke(this, ToolMode.None);
            return;
        }

        _penSelected = true;
        ToolSelected?.Invoke(this, ToolMode.Pen);
    }
}