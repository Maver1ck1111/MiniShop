using System.Collections.Generic;
using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MiniShop.ObservableStateServices;

namespace MiniShop.Tools.Common;

public abstract class Tool : ITool
{
    protected Point _startPosition;
    protected ILockedFramebuffer? _lockedBitMap;
    protected bool _isPanning = false;
    protected List<Point> _points = new();
    protected WriteableBitmap _bitmap;
    protected readonly ILayerStateService _layerStateService;
    
    public Tool(ILayerStateService layerStateService)
    {
        _layerStateService = layerStateService;
        _bitmap = _layerStateService.SelectedLayer;
        _layerStateService.LayerChanged += OnLayerChanged;
    }

    private void OnLayerChanged()
    {
        _bitmap = _layerStateService.SelectedLayer;
    }
    
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
