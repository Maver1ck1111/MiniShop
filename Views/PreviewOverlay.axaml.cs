using Avalonia.Controls;
using Avalonia.Media;
using MiniShop.Tools;

namespace MiniShop.Views;

public partial class PreviewOverlay : Control
{
    public ITool? SelectedTool { get; set; }
    
    public PreviewOverlay()
    {
        IsHitTestVisible = false;
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
        SelectedTool?.DrawPreview(context);
    }
}