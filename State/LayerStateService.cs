using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Media.Imaging;

namespace MiniShop.ObservableStateServices;

public class LayerStateService : ILayerStateService
{
    public WriteableBitmap? SelectedLayer { get; private set; }

    public IReadOnlyList<WriteableBitmap> Layers { get => _layers.AsReadOnly(); }

    public WriteableBitmap? Image { get; private set; }

    private List<WriteableBitmap> _layers { get; set; } = new List<WriteableBitmap>();
    
    public event Action LayerChanged;
    public event Action<WriteableBitmap> ImageSelected;
    public event Action LayerUpdated;

    public void SetImage(WriteableBitmap image)
    {
        Image = image;
        ImageSelected.Invoke(Image);
    }

    public void SetLayer(WriteableBitmap layer)
    {
        if (!Layers.Contains(layer)) return;
        SelectedLayer = layer;
        LayerChanged.Invoke();
    }

    public void SetNewLayer(WriteableBitmap layer)
    {
        SelectedLayer = layer;
        _layers.Add(SelectedLayer);
        LayerChanged.Invoke();
    }

    public void UpdateSelectedLayer()
    {
        if (SelectedLayer is not null)
            LayerUpdated.Invoke();
    }
}
