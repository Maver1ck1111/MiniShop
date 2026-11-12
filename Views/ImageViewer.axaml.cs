using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using MiniShop.Enums;
using MiniShop.Factories;
using MiniShop.Tools;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace MiniShop;

public partial class ImageViewer : UserControl
{
    private readonly double _zoomFactor = 1.15;
    private double _currentScale = 1;
    private double _actualWidth;
    private double _actualHeight;
    private Point _startPoint;
    private bool _isPanning = false;

    private readonly ScaleTransform _scaleTransform = new(1, 1);
    private readonly TranslateTransform _translateTransform = new(0, 0);
    private ITool? _tool = null;
    private ToolsFactory _factory;

    public ImageViewer()
    {
        InitializeComponent();
        _factory = new ToolsFactory();
    }
    public void SetTool(object? sender, ToolMode toolMode)
    {
        if (Image.Source is WriteableBitmap bitmap)
            _tool = _factory.CreateTool(toolMode, bitmap);
    }
    private async void OnSelectImageClicked(object? sender, RoutedEventArgs e)
    {
        var toplevel = TopLevel.GetTopLevel(this);
        if (toplevel == null) return;

        var files = await toplevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            AllowMultiple = false,
            Title = "Choose your pictire",
            FileTypeFilter = new[]
            {
                FilePickerFileTypes.ImageAll
            }
        });

        if (files.Count > 0)
        {
            var file = files[0];

            await using var stream = await file.OpenReadAsync();

            var originalBitmap = new Bitmap(stream);

            var writeableBitmap = new WriteableBitmap(
                new PixelSize((int)originalBitmap.Size.Width, (int)originalBitmap.Size.Height),
                new Vector(96, 96),
                PixelFormat.Bgra8888,
                AlphaFormat.Premul
            );

            using (var lockedFrame = writeableBitmap.Lock())
            {
                originalBitmap.CopyPixels(
                    new PixelRect(0, 0, writeableBitmap.PixelSize.Width, writeableBitmap.PixelSize.Height),
                    lockedFrame.Address,
                    lockedFrame.Size.Height * lockedFrame.RowBytes,
                    lockedFrame.RowBytes
                );
            }

            Image.Source = writeableBitmap;

            SelectImageButton.IsVisible = false;

            _actualHeight = originalBitmap.Size.Height;
            _actualWidth = originalBitmap.Size.Width;

            Loaded += (s, e) => CenterImage();
            ViewportBorder.SizeChanged += (s, e) => CenterImage();

            var group = new TransformGroup();
            group.Children.Add(_scaleTransform);
            group.Children.Add(_translateTransform);

            ImageCanvas.RenderTransform = group;

            CenterImage();
        }
    }

    private void OnCanvasPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        double oldScale = _scaleTransform.ScaleX;

        double zoom = e.Delta.Y > 0 ? _zoomFactor : (1 / _zoomFactor);
        double newScale = oldScale * zoom;

        if (newScale < 0.1 || newScale > 20.0) return;

        var mousePos = e.GetPosition(ViewportBorder);

        _scaleTransform.ScaleX = newScale;
        _scaleTransform.ScaleY = newScale;

        _currentScale = newScale;

        if(Image.Source is not Bitmap bitmap) return;

        _actualWidth = bitmap.Size.Width * newScale;
        _actualHeight = bitmap.Size.Height * newScale;

        CenterImage();

        e.Handled = true;
    }
    private void OnCanvasPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (_tool is null)
        {
            DefaultPointerPressedBehaviour(sender, e);
            return;
        }

        _tool.OnPressed(sender, e);
    }
    private void OnCanvasPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_tool is null)
        {
            DefaultPointerReleasedBehaviour(sender, e);
            return;
        }

        _tool.OnReleased(sender, e);
    }

    private void OnCanvasPointerMoved(object? sender, PointerEventArgs e)
    {
        if (_tool is null)
        {
            DefaultPointerMovedBehaviour(sender, e);
            return;
        }

        var mousePos = e.GetPosition(ViewportBorder);

        double pointX = Math.Abs(Math.Abs(_translateTransform.X) - mousePos.X) / _currentScale;
        double pointY = Math.Abs(Math.Abs(_translateTransform.Y) - mousePos.Y) / _currentScale;

        var actualPoin = new Point(pointX, pointY);

        _tool.OnMoved(sender, e, actualPoin);
        Image.InvalidateVisual();
    }

    private void DefaultPointerMovedBehaviour(object? sender, PointerEventArgs e)
    {
        if (!_isPanning) return;
        if (IsTheHolePictureOnTheScreen()) return;

        var currentPoint = e.GetPosition(ViewportBorder);
        var delta = currentPoint - _startPoint;

        _translateTransform.X += delta.X;
        _translateTransform.Y += delta.Y;

        CenterImage();

        _startPoint = currentPoint;
    }
    private void DefaultPointerReleasedBehaviour(object? sender, PointerReleasedEventArgs e)
    {
        if (_isPanning)
        {
            _isPanning = false;
            e.Pointer.Capture(null);
        }
    }
    private void DefaultPointerPressedBehaviour(object? sender, PointerPressedEventArgs e)
    {
        var properties = e.GetCurrentPoint(ViewportBorder).Properties;

        if (!properties.IsMiddleButtonPressed) return;

        _isPanning = true;
        _startPoint = e.GetPosition(ViewportBorder);
        e.Pointer.Capture(ViewportBorder);
    }
    private void CenterImage()
    {
        double viewportWidth = ViewportBorder.Bounds.Width;
        double viewportHeight = ViewportBorder.Bounds.Height;

        if (_actualWidth <= viewportWidth)
        {
            _translateTransform.X = (viewportWidth - _actualWidth) / 2;
        }
        else
        {
            double minX = viewportWidth - _actualWidth;
            double maxX = 0;
            _translateTransform.X = Math.Clamp(_translateTransform.X, minX, maxX);
        }

        if (_actualHeight <= viewportHeight)
        {
            _translateTransform.Y = (viewportHeight - _actualHeight) / 2;
        }
        else
        {
            double minY = viewportHeight - _actualHeight;
            double maxY = 0;
            _translateTransform.Y = Math.Clamp(_translateTransform.Y, minY, maxY);
        }
    }

    private bool IsTheHolePictureOnTheScreen()
    {
        return _actualWidth < ViewportBorder.Bounds.Width && _actualHeight < ViewportBorder.Bounds.Height;
    }
}
