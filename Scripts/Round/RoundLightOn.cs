using Godot;
using Godot.Collections;

public partial class RoundLightOn : Node
{
    [Export] private Array<Light3D> _mapLights;

    public override void _Ready()
    {
        GameEvent.Instance.ShieldFixed += TurnOnMapLights;
    }

    public override void _ExitTree()
    {
        GameEvent.Instance.ShieldFixed -= TurnOnMapLights;
    }

    private void TurnOnMapLights()
    {
        foreach (var light in _mapLights)
            GlobalRpcFunctions.Instance.SetVisibility(light.GetPath(), true);
    }
}
