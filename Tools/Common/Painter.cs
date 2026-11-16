using Avalonia.Platform;

namespace MiniShop.Tools.Common;

public static class Painter
{
        public static void SetPixel(int x, int y, ILockedFramebuffer lockedBitmap, Pixel pixel)
        {
            byte R = pixel.R;
            byte G = pixel.G;
            byte B = pixel.B;

            int rowBytes = lockedBitmap.RowBytes;

            nint ptr = lockedBitmap.Address;

            unsafe
            {
                byte* rowPointer = (byte*)ptr + y * rowBytes;
                byte* pixelPointer = rowPointer + x * 4;

                byte bgB = pixelPointer[0];
                byte bgG = pixelPointer[1];
                byte bgR = pixelPointer[2];
                byte bgA = pixelPointer[3];

                double srcAlpha = pixel.Opacity / 255.0;
                double invSrcAlpha = 1.0 - srcAlpha;
                
                byte newB = (byte)(B * srcAlpha + bgB * invSrcAlpha);
                byte newG = (byte)(G * srcAlpha + bgG * invSrcAlpha);
                byte newR = (byte)(R * srcAlpha + bgR * invSrcAlpha);

                double bgAlphaNorm = bgA / 255.0;
                byte newA = (byte)((srcAlpha + bgAlphaNorm * invSrcAlpha) * 255);

                pixelPointer[0] = newB;
                pixelPointer[1] = newG;
                pixelPointer[2] = newR;
                pixelPointer[3] = newA;
            }
        }
}
