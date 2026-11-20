using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace MiniShop.Tools
{
    public interface ITool
    {
        void OnPressed(object? sender, PointerPressedEventArgs e, Point position);
        void OnMoved(object? sender, RoutedEventArgs e, Point position);
        void OnReleased(object? sender, RoutedEventArgs e, Point position);
        void DrawPreview(DrawingContext context);
    }
}
