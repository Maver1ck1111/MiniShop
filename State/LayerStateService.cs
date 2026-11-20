using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;

namespace MiniShop.ObservableStateServices;

public class LayerStateService : ILayerStateService
{
    public WriteableBitmap? SelectedLayer { get; private set; }

    public IReadOnlyList<WriteableBitmap> Layers { get => _layers.AsReadOnly(); }

    private List<WriteableBitmap> _layers { get; set; } = new List<WriteableBitmap>();
    
    public event Action<WriteableBitmap> LayerChanged;

    public void SetLayer(WriteableBitmap layer)
    {
        if (!Layers.Contains(layer)) return;
        SelectedLayer = layer;
    }

    public void SetNewLayer(WriteableBitmap layer)
    {
        SelectedLayer = layer;
        _layers.Add(SelectedLayer);
        LayerChanged.Invoke(SelectedLayer);
    }
}
