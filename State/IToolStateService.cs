using System;
using MiniShop.Tools;

namespace MiniShop.ObservableStateServices;

public interface IToolStateService
{
    ITool? SelectedTool { get; }
    event Action<ToolMode> ToolChanged;
    void SetTool(ToolMode mode);
}
