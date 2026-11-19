using System;
using MiniShop.Tools;

namespace MiniShop.ObservableStateServices;

public interface IToolsStateService
{
    ITool? SelectedTool { get; }
    event Action<ToolMode> ToolChanged;
    void SetTool(ToolMode mode);
}
