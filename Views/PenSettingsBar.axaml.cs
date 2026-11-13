using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Tools;
using MiniShop.Tools.Pen;
using System;
using System.Drawing;

namespace MiniShop.Views;

public partial class PenSettingsBar : UserControl
{
    private PenSettings _penSettings;
    public PenSettingsBar()
    {
        InitializeComponent();

        _penSettings = App.ServiceProvider.GetService<ToolsSettings>()?.PenSettings;
        PenSizeComboBox.SelectedIndex = 0;
        PenColorComboBox.SelectedIndex = 0;
        OpacityTexBox.Text = "100";
    }

    private void PenPixelSizeChangedBox(object? sender, SelectionChangedEventArgs e)
    {
        if (PenSizeComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string? pixelSize = selectedItem.Content?.ToString();
            _penSettings.PixelSize = int.Parse(pixelSize);
        }
    }

    private void PenColorChangedBox(object? sender, SelectionChangedEventArgs e)
    {
        if (PenColorComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string? penColor = selectedItem.Content?.ToString();
            if (Enum.TryParse<KnownColor>(penColor, out var knownKolor))
            {
                var color = Color.FromKnownColor(knownKolor);
                _penSettings.Color = (color.R, color.G, color.B);
            }
        }
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBlock)
        {
            var text = textBlock.Text;

            if (int.TryParse(text, out var result) && result <= 100 && result >= 0)
            {
                _penSettings.Opacity = (byte)((double)255 / 100 * result);
                return;
            }

            textBlock.Text = "100";
        }
    }
}