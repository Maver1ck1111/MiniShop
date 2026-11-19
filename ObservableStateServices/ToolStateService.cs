using System;
using MiniShop.Factories;
using MiniShop.Tools;

namespace MiniShop.ObservableStateServices;

public class ToolStateService(IToolsFactory _factory) : IToolsStateService
{
    public ITool? SelectedTool { get; private set;}

    public event Action<ToolMode> ToolChanged;

    public void SetTool(ToolMode mode)
    {
        SelectedTool = _factory.CreateTool(mode);
        ToolChanged?.Invoke(mode);
    }
}
