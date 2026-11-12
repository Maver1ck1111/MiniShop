using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace MiniShop.Tools
{
    public interface ITool
    {
        void OnPressed(object? sender, PointerPressedEventArgs e, Point? position = null);
        void OnMoved(object? sender, RoutedEventArgs e, Point position);
        void OnReleased(object? sender, RoutedEventArgs e, Point? position = null);
    }
}
