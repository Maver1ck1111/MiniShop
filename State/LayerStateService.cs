using System;
using Avalonia.Media.Imaging;

namespace MiniShop.ObservableStateServices;

public class LayerStateService : ILayerStateService
{
    public WriteableBitmap? SelectedLayer { get; private set; }

    public event Action<WriteableBitmap> LayerChanged;

    public void SetLayer(WriteableBitmap layer)
    {
        SelectedLayer = layer;
        LayerChanged.Invoke(SelectedLayer);
    }
}
