using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System;

namespace MiniShop.Tools
{
    public class Pen(WriteableBitmap _bitmap) : ITool
    {
        private bool _isPanning = false;
        public void OnMoved(object? sender, RoutedEventArgs e, Point position)
        {
            if (!_isPanning) return;

            using (var lockedBitmap = _bitmap.Lock())
            {
                int rowBytes = lockedBitmap.RowBytes;

                IntPtr ptr = lockedBitmap.Address;

                unsafe
                {
                    byte* rowPointer = (byte*)ptr + ((int)position.Y * rowBytes);
                    byte* pixelPointer = rowPointer + ((int)position.X * 4);

                    pixelPointer[0] = 0;
                    pixelPointer[1] = 0;
                    pixelPointer[2] = 0;
                    pixelPointer[3] = 255;
                }
            }
        }

        public void OnPressed(object? sender, PointerPressedEventArgs e, Point? position)
        {
            var properties = e.GetCurrentPoint(sender as Visual).Properties;

            if (!properties.IsLeftButtonPressed) return;

            _isPanning = true;
        }

        public void OnReleased(object? sender, RoutedEventArgs e, Point? position)
        {
            _isPanning = false;
        }
    }
}
