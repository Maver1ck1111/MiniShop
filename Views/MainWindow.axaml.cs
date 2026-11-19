using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.ObservableStateServices;
using MiniShop.Tools;

namespace MiniShop.Views;

public partial class MainWindow : Window
{
    private readonly IToolsStateService _toolsService;
    public MainWindow()
    {
        InitializeComponent();

        _toolsService = App.ServiceProvider.GetRequiredService<IToolsStateService>();
        _toolsService.ToolChanged += UpdateToolsSettingsVisibility;
    }

    public void UpdateToolsSettingsVisibility(ToolMode toolMode)
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