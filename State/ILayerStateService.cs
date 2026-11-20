using System;
using System.Collections.Generic;
using Avalonia.Media.Imaging;

namespace MiniShop.ObservableStateServices;

public interface ILayerStateService
{
    WriteableBitmap? SelectedLayer { get; }
    IReadOnlyList<WriteableBitmap> Layers { get; }
    event Action<WriteableBitmap> LayerChanged;
    void SetNewLayer(WriteableBitmap layer);
    void SetLayer(WriteableBitmap layer);
}
