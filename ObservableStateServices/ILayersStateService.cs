using System;
using Avalonia.Media.Imaging;

namespace MiniShop.ObservableStateServices;

public interface ILayersStateService
{
    WriteableBitmap? SelectedLayer { get; }
    event Action<WriteableBitmap> LayerChanged;
    void SetLayer(WriteableBitmap layer);
}
