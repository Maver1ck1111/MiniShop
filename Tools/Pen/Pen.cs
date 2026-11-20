using Avalonia;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using MiniShop.ObservableStateServices;
using MiniShop.Tools.Common;
using System;

namespace MiniShop.Tools.Pen
{
    public class Pen(PenSettings _settings, ILayerStateService _layerService) : Tool(_layerService.SelectedLayer)
    {   
        protected override void Paint(Point startPosition, Point currentPosition)
        {
            double radius = Math.Sqrt(_settings.PixelSize);

            if (!double.IsInteger(radius)) return;

            int intRadius = (int) radius;

            int bitmapWidth = _layerService.SelectedLayer.PixelSize.Width;
            int bitmapHeight = _layerService.SelectedLayer.PixelSize.Height;

            int centerX = (int) currentPosition.X;
            int centerY = (int) currentPosition.Y;

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
                            Painter.SetPixel(targetX, targetY, _lockedBitMap, _settings.Pixel);
                        }
                    }
                }
            }
        }
    }
}
