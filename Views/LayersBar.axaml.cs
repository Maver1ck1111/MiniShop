using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.ObservableStateServices;

namespace MiniShop.Views;

public partial class LayersBar : UserControl
{
    private Bitmap _originalPicture;
    private readonly List<WriteableBitmap> _layers = new();
    private ILayerStateService _layerService;
    
    public LayersBar()
    {
        InitializeComponent();
        _layerService = App.ServiceProvider.GetRequiredService<ILayerStateService>();
    }
    
    public void OnImageSelected(object? sender, Bitmap picture)
    {
        _originalPicture = picture;
        int width = (int) _originalPicture.Size.Width;
        int height = (int) _originalPicture.Size.Height;
        int stride = width * 4;

        var writeableBitmap = new WriteableBitmap(
            new PixelSize((int)_originalPicture.Size.Width, (int)_originalPicture.Size.Height),
            new Vector(96, 96),
            PixelFormat.Bgra8888,
            AlphaFormat.Premul
        );

        using (var lockedFrame = writeableBitmap.Lock())
        {
            _originalPicture.CopyPixels(
                new PixelRect(0, 0, writeableBitmap.PixelSize.Width, writeableBitmap.PixelSize.Height),
                lockedFrame.Address,
                lockedFrame.Size.Height * lockedFrame.RowBytes,
                lockedFrame.RowBytes
            );
        }
            
        _layers.Add(writeableBitmap);
            
        _layerService.SetLayer(writeableBitmap);
    }
}