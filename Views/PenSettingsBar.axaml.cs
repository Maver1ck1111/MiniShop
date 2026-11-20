using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.Extensions.DependencyInjection;
using MiniShop.Tools;
using MiniShop.Tools.Pen;
using System;
using System.Drawing;
using System.Linq;

namespace MiniShop.Views;

public partial class PenSettingsBar : UserControl
{
    private readonly PenSettings _penSettings;
    private bool _isUpdatingText;

    public PenSettingsBar()
    {
        InitializeComponent();

        _penSettings = App.ServiceProvider.GetRequiredService<ToolsSettings>().PenSettings;
        PenColorComboBox.SelectedIndex = 0;
        OpacityTexBox.Text = "100";
        PenSizeBox.Text = "1";
    }

    private void OnSizeChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isUpdatingText || sender is not TextBox textBox) return;

        int? value = SanitizeNumericInput(textBox, min: 1, max: 100);
        if (value.HasValue)
        {
            _penSettings.PenThickness = value.Value;
        }
    }

    private void OnSizeLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;

        if (!int.TryParse(textBox.Text, out int value) || value < 1)
        {
            SetTextSafely(textBox, "1");
            _penSettings.PenThickness = 1;
        }
    }

    private void PenColorChangedBox(object? sender, SelectionChangedEventArgs e)
    {
        if (PenColorComboBox.SelectedItem is ComboBoxItem selectedItem)
        {
            string? penColor = selectedItem.Content?.ToString();
            if (Enum.TryParse<KnownColor>(penColor, out var knownColor))
            {
                var color = Color.FromKnownColor(knownColor);
                _penSettings.Pixel.R = color.R;
                _penSettings.Pixel.G = color.G;
                _penSettings.Pixel.B = color.B;
            }
        }
    }

    private void OnOpacityChanged(object? sender, TextChangedEventArgs e)
    {
        if (_isUpdatingText || sender is not TextBox textBox) return;

        int? value = SanitizeNumericInput(textBox, min: 0, max: 100);
        if (value.HasValue)
        {
            _penSettings.Pixel.Opacity = (byte)(255.0 / 100 * value.Value);
        }
    }

    private void OnOpacityLostFocus(object? sender, RoutedEventArgs e)
    {
        if (sender is not TextBox textBox) return;

        if (!int.TryParse(textBox.Text, out _))
        {
            SetTextSafely(textBox, "100");
            _penSettings.Pixel.Opacity = 255;
        }
    }

    private int? SanitizeNumericInput(TextBox textBox, int min, int max)
    {
        string raw = textBox.Text ?? string.Empty;
        string digitsOnly = string.Concat(raw.Where(char.IsDigit));

        if (digitsOnly.Length == 0)
        {
            if (raw.Length > 0) SetTextSafely(textBox, string.Empty);
            return null;
        }

        int value = int.Parse(digitsOnly);
        if (value > max)
        {
            value = max;
            SetTextSafely(textBox, max.ToString());
        }
        else if (digitsOnly != raw)
        {
            SetTextSafely(textBox, digitsOnly);
        }

        return Math.Clamp(value, min, max);
    }

    private void SetTextSafely(TextBox textBox, string text)
    {
        _isUpdatingText = true;
        textBox.Text = text;
        textBox.CaretIndex = text.Length;
        _isUpdatingText = false;
    }
}