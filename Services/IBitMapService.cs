using Avalonia.Media.Imaging;

namespace MiniShop.Services;

public interface IBitMapService
{
    WriteableBitmap CreateEmptyMap(WriteableBitmap bitmap);
}
