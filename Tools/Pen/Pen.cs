using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using System;

namespace MiniShop.Tools.Pen
{
    public class Pen(PenSettings _settings, WriteableBitmap _bitmap) : ITool
    {
        private bool _isPanning = false;
        public void OnMoved(object? sender, RoutedEventArgs e, Point position)
        {
            if (!_isPanning) return;

            double radius = Math.Sqrt(_settings.PixelSize);

            if (!double.IsInteger(radius)) return;

            int intRadius = (int) radius;

            int bitmapWidth = _bitmap.PixelSize.Width;
            int bitmapHeight = _bitmap.PixelSize.Height;

            int centerX = (int) position.X;
            int centerY = (int) position.Y;

            for (int y = -intRadius; y <= radius; y++)
            {
                for (int x = -intRadius; x <= radius; x++)
                {
                    if (x * x + y * y <= radius * radius)
                    {
                        int targetX = centerX + x;
                        int targetY = centerY + y;

                        if (targetX >= 0 && targetX < bitmapWidth && targetY >= 0 && targetY < bitmapHeight)
                        {
                            SetPixel(targetX, targetY);
                        }
                    }
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

        private void SetPixel(int x, int y)
        {
            byte R = _settings.Color.Item1;
            byte G = _settings.Color.Item2;
            byte B = _settings.Color.Item3;

            using (var lockedBitmap = _bitmap.Lock())
            {
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

                    double srcAlpha = _settings.Opacity / 255.0;
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
    }
}
