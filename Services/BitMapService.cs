using System;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace MiniShop.Services;

public class BitMapService : IBitMapService
{
    public WriteableBitmap CreateEmptyMap(WriteableBitmap bitmap)
    {
         var clearBitmap = new WriteableBitmap(
            new PixelSize((int)bitmap.Size.Width, (int)bitmap.Size.Height),
            new Vector(96, 96),
            PixelFormat.Bgra8888,
            AlphaFormat.Premul
        );

        using (var lockedFrame = clearBitmap.Lock())
        {
            int totalBytes = lockedFrame.Size.Height * lockedFrame.RowBytes;

            unsafe
            {
                var buffer = new Span<byte>((void*)lockedFrame.Address, totalBytes);
                buffer.Clear();
            }
        }
        
        return clearBitmap;
    }
}
