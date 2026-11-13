using Avalonia.Controls;
using MiniShop.Tools;

namespace MiniShop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ToolsBar.ToolSelected += ImageViewer.SetTool;
        ToolsBar.ToolSelected += UpdateToolsSettingsVisibility;
    }

    public void UpdateToolsSettingsVisibility(object? sender, ToolMode toolMode)
    {
        if (toolMode == ToolMode.None)
        {
            MainGrid.ColumnDefinitions[2].Width = new GridLength(0);
            SettingsBar.IsVisible = false;
        } 
        else
        {
            MainGrid.ColumnDefinitions[2].Width = new GridLength(1, GridUnitType.Star);
            SettingsBar.IsVisible = true;
        }
    }

}