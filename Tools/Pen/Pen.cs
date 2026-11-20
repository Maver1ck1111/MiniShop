using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Media;
using MiniShop.ObservableStateServices;
using MiniShop.Tools.Common;
using System;
using System.Collections.Generic;

namespace MiniShop.Tools.Pen;

public class Pen : Tool
{
    private readonly PenSettings _settings;
    private readonly ILayerStateService _layerStateSetvice;
        
    public Pen(PenSettings settings,
          ILayerStateService layerStateSetvice) 
          : base(layerStateSetvice.SelectedLayer)
    {
        _settings = settings;
        _layerStateSetvice = layerStateSetvice;
    }

    public override void DrawPreview(DrawingContext context)
    {
        var pixel = _settings.Pixel;
        var thickness = _settings.PenThickness;
        
        var color = Color.FromArgb(pixel.Opacity, pixel.R, pixel.G, pixel.B); // A, R, G, B (0..255)
        var brush = new SolidColorBrush(color);
        
        var geometry = new PolylineGeometry(_points, isFilled: false);
        var pen = new Avalonia.Media.Pen(
            brush: brush,
            thickness: thickness,
            lineCap: PenLineCap.Round,
            lineJoin: PenLineJoin.Round
        );

        context.DrawGeometry(brush: null, pen: pen, geometry: geometry);
    }

    public override void OnPressed(object? sender, PointerPressedEventArgs e, Point position)
    {
        base.OnPressed(sender, e, position);
    }
    
 
    protected override void CreatePointsForDrawing(List<Point> points)
    {
        if (points.Count == 0) return;

        var allPoints = new List<Point>
        {
            new Point(Math.Round(points[0].X), Math.Round(points[0].Y))
        };

        for (int i = 1; i < points.Count; i++)
        {
            int x0 = (int)Math.Round(points[i - 1].X);
            int y0 = (int)Math.Round(points[i - 1].Y);
            int x1 = (int)Math.Round(points[i].X);
            int y1 = (int)Math.Round(points[i].Y);

            int dx = x1 - x0;
            int dy = y1 - y0;

            int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
            if (steps == 0) continue;

            double xStep = (double)dx / steps;
            double yStep = (double)dy / steps;

            double x = x0;
            double y = y0;

            for (int s = 0; s < steps; s++)
            {
                x += xStep;
                y += yStep;
                allPoints.Add(new Point(Math.Round(x), Math.Round(y)));
            }
        }

        Paint(allPoints);
    }

    protected override void Paint(List<Point> points)
    {
        int bitmapWidth = _layerStateSetvice.SelectedLayer.PixelSize.Width;
        int bitmapHeight = _layerStateSetvice.SelectedLayer.PixelSize.Height;

        int radius = _settings.PenThickness / 2;
        foreach (var point in points)
        {
            int centerX = (int) point.X;
            int centerY = (int) point.Y;

            for (int y = -radius; y <= radius; y++)
            {
                for (int x = -radius; x <= radius; x++)
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
