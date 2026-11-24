using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace MiniShop.ObservableStateServices;

public interface ILayerStateService
{
    WriteableBitmap? SelectedLayer { get; }
    WriteableBitmap? Image { get; }
    IReadOnlyList<WriteableBitmap> Layers { get; }
    event Action LayerChanged;
    event Action<WriteableBitmap> ImageSelected;
    event Action LayerUpdated;
    void UpdateSelectedLayer();
    void SetNewLayer(WriteableBitmap layer);
    void SetLayer(WriteableBitmap layer);
    void SetImage(WriteableBitmap image);
}
