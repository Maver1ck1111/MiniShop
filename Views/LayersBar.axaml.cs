using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.ObservableStateServices;
using MiniShop.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MiniShop.Views;

public partial class LayersBar : UserControl
{
    private readonly ObservableCollection<LayerUiItem> _layers = new();
    private WriteableBitmap _image;
    private int _layerCounter = 1;

    private readonly ILayerStateService _layerStateService;
    private readonly IBitMapService _bitMapService;

    public LayersBar()
    {
        InitializeComponent();
        _layerStateService = App.ServiceProvider.GetRequiredService<ILayerStateService>();
        _bitMapService = App.ServiceProvider.GetRequiredService<IBitMapService>();

        _layerStateService.ImageSelected += OnImageSelected;
        _layerStateService.LayerUpdated += OnUpdatedSelecedLayer;
    }

    private void OnUpdatedSelecedLayer()
    {
        foreach (var img in LayersListControl.GetVisualDescendants().OfType<Image>())
        {
            if (ReferenceEquals(img.Source, _layerStateService.SelectedLayer))
            {
                img.InvalidateVisual();
            }
        }
    }

    private void OnImageSelected(WriteableBitmap imageBitmab)
    {
        _image = imageBitmab;
        var currentLayer = _bitMapService.CreateEmptyMap(_image);
        _layerStateService.SetNewLayer(currentLayer);
        
        _layers.Add(new LayerUiItem("Image", "Background", false, _image));
        _layers.Add(new LayerUiItem($"Layer {++_layerCounter}", "Writable", true, currentLayer));
        
        LayersListControl.ItemsSource = _layers;
    }

    private void OnLayerRowClicked(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not Border { DataContext: LayerUiItem clicked }) return;
        
        foreach (var layer in _layers)
        {
            if (ReferenceEquals(layer, clicked))
            {
                layer.IsSelected = !layer.IsSelected;
                _layerStateService.SetLayer(layer.Bitmap);
            }
        }
    }

    private void OnToggleVisibilityClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: LayerUiItem layer })
        {
            layer.IsLayerVisible = !layer.IsLayerVisible;
            _layerStateService.ToggleLayerVisibility(layer.Bitmap, layer.IsLayerVisible);
        }
    }

    private void OnAddLayerClicked(object? sender, RoutedEventArgs e)
    {
        foreach (var layer in _layers)
        {
            layer.IsSelected = false;
        }

        var currentLayer = _bitMapService.CreateEmptyMap(_image);
        _layerStateService.SetNewLayer(currentLayer);
        
        _layers.Add(new LayerUiItem($"Layer {++_layerCounter}", "Drawing", true, currentLayer));
    }
}

public class LayerUiItem : INotifyPropertyChanged
{
    private bool _isSelected;
    private bool _isLayerVisible = true;
    public string Name { get; }
    public string Kind { get; }
    public WriteableBitmap Bitmap { get; }
    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            if (_isSelected == value) return;
            _isSelected = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
        }
    }
    public bool IsLayerVisible
    {
        get => _isLayerVisible;
        set
        {
            if (_isLayerVisible == value) return;
            _isLayerVisible = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsLayerVisible)));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    public LayerUiItem(string name, string kind, bool isSelected, WriteableBitmap bitmap)
    {
        Name = name;
        Kind = kind;
        _isSelected = isSelected;
        Bitmap = bitmap;
    }
}