using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;

namespace MiniShop.Services;

public class DialogService : IDialogService
{
    public async Task<WriteableBitmap?> SelectImage(Visual visual)
    {
        var toplevel = TopLevel.GetTopLevel(visual);
        if (toplevel == null) return null;

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
            
            return writeableBitmap;
        }
        
        return null;
    }
}
