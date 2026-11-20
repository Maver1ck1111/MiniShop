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
        PenColorComboBox.SelectedIndex = 0;
        OpacityTexBox.Text = "100";
        PenSizeBox.Text = "1";
    }

    private void OnSizeChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBox)
        {
            if (int.TryParse(textBox.Text, out var result))
            {
                _penSettings.PenThickness = result;
                return;
            }
            
            textBox.Text = "1";
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
                _penSettings.Pixel.R = color.R;
                _penSettings.Pixel.G = color.G;
                _penSettings.Pixel.B = color.B;
            }
        }
    }

    private void OnOpacityChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is TextBox textBlock)
        {
            var text = textBlock.Text;

            if (int.TryParse(text, out var result) && result <= 100 && result >= 0)
            {
                _penSettings.Pixel.Opacity = (byte)((double)255 / 100 * result);
                return;
            }

            textBlock.Text = "100";
        }
    }
}