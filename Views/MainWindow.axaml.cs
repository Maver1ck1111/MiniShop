using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using MiniShop.Enums;

namespace MiniShop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        ToolsBar.ToolSelected += ImageViewer.SetTool;
    }

}