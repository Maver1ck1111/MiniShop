using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Factories;
using MiniShop.ObservableStateServices;
using MiniShop.Services;
using MiniShop.Tools;
using MiniShop.Views;
using System;

namespace MiniShop;

public partial class App : Application
{
    public static IServiceProvider ServiceProvider { get; private set; }
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);

        var services = new ServiceCollection();

        services.AddTransient<IToolsFactory, ToolsFactory>();
        services.AddTransient<IDialogService, DialogService>();
        services.AddSingleton<ToolsSettings>();

        services.AddSingleton<IToolStateService, ToolStateService>();
        services.AddSingleton<ILayerStateService, LayerStateService>();
        ServiceProvider = services.BuildServiceProvider();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}