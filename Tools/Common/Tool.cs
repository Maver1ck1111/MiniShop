using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace MiniShop.Tools.Common;

public abstract class Tool(WriteableBitmap _bitmap) : ITool
{
    protected Point _startPosition;
    protected ILockedFramebuffer? _lockedBitMap;
    protected bool _isPanning = false;
    
    public virtual void OnMoved(object? sender, RoutedEventArgs e, Point position)
    {
        if (!_isPanning) return;
        
        using (var locked = _bitmap.Lock())
        {
            _lockedBitMap = locked;
            Paint(_startPosition, position);
        }
    }

    public virtual void OnPressed(object? sender, PointerPressedEventArgs e, Point position)
    {
        _isPanning = true;
        _startPosition = position;    
    }

    public virtual void OnReleased(object? sender, RoutedEventArgs e)
    {
        _isPanning = false;
    }
    
    protected abstract void Paint(Point startPosition, Point currentPosition);
}
