using Avalonia.Platform;

namespace MiniShop.Tools.Common;

public static class Painter
{
        public static void SetPixel(int x, int y, ILockedFramebuffer lockedBitmap, Pixel pixel)
        {
            int rowBytes = lockedBitmap.RowBytes;

            nint ptr = lockedBitmap.Address;

            unsafe
            {
                byte* rowPointer = (byte*)ptr + y * rowBytes;
                byte* pixelPointer = rowPointer + x * 4;

                pixelPointer[0] = pixel.B;
                pixelPointer[1] = pixel.G;
                pixelPointer[2] = pixel.R;
                pixelPointer[3] = pixel.Opacity;
            }
        }
}
