using System.Threading.Tasks;
using Avalonia;
using Avalonia.Media.Imaging;

namespace MiniShop.Services;

public interface IDialogService
{
    Task<WriteableBitmap?> SelectImage(Visual visual);
}
