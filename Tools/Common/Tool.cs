using System.Collections.Generic;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace MiniShop.Tools.Common;

public abstract class Tool(WriteableBitmap _bitmap) : ITool
{
    protected Point _startPosition;
    protected ILockedFramebuffer? _lockedBitMap;
    protected bool _isPanning = false;
    protected List<Point> _points = new();
    
    public virtual void OnMoved(object? sender, RoutedEventArgs e, Point position)
    {
        if (!_isPanning) return;
        _points.Add(position);
    }

    public virtual void OnPressed(object? sender, PointerPressedEventArgs e, Point position)
    {
        _isPanning = true;
        _points.Add(position);
    }

    public virtual void OnReleased(object? sender, RoutedEventArgs e, Point position)
    {
        _isPanning = false;
        
        _points.Add(position);
        
        using (var locked = _bitmap.Lock())
        {
            _lockedBitMap = locked;
            CreatePointsForDrawing(_points);
        }
        
        _points.Clear();
    }
    
    protected abstract void Paint(List<Point> points);
    protected abstract void CreatePointsForDrawing(List<Point> points);
    public abstract void DrawPreview(DrawingContext context);
}
