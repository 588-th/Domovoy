using Godot;

public partial class PlayerPropDrag
{
    private PlayerPropInteractor _playerPropInteractor;
    private Node3D _lookedObject;

    public PlayerPropDrag(PlayerPropInteractor playerPropInteractor)
    {
        _playerPropInteractor = playerPropInteractor;
    }

    public void OnInteractKeyPressed()
    {
        _lookedObject = (Node3D)_playerPropInteractor.RayOfLook.GetCollider();

        if (_playerPropInteractor.DraggedObject == null && _lookedObject?.IsInGroup("Prop:Draggble") == true && !_lookedObject.IsInGroup("Prop:Dragged"))
        {
            StartDragging();
        }
        else if (_playerPropInteractor.DraggedObject != null)
        {
            StopDragging();
        }
    }

    public void StartDragging()
    {
        _playerPropInteractor.DraggedObject = _lookedObject as RigidBody3D;
        _playerPropInteractor.DraggedObject.AngularVelocity = Vector3.Zero;
        _playerPropInteractor.DraggedObject.LinearVelocity = Vector3.Zero;
        _playerPropInteractor.DraggedObject.GravityScale = 0;
        _playerPropInteractor.DraggedObject.AddCollisionExceptionWith(_playerPropInteractor.PlayerBody);
        _playerPropInteractor.Generic6DofJoint3D.NodeB = _playerPropInteractor.DraggedObject.GetPath();

        RpcFunctions.Instance.AddGroup(_playerPropInteractor.DraggedObject.GetPath(), "Prop:Dragged");
    }

    public void StopDragging()
    {
        RpcFunctions.Instance.RemoveGroup(_playerPropInteractor.DraggedObject.GetPath(), "Prop:Dragged");

        _playerPropInteractor.DraggedObject.AngularVelocity = Vector3.Zero;
        _playerPropInteractor.DraggedObject.LinearVelocity = Vector3.Zero;
        _playerPropInteractor.DraggedObject.LinearVelocity = _playerPropInteractor.CalculateInertia();
        _playerPropInteractor.DraggedObject.GravityScale = 1;
        _playerPropInteractor.DraggedObject.RemoveCollisionExceptionWith(_playerPropInteractor.PlayerBody);
        _playerPropInteractor.DraggedObject = null;
        _playerPropInteractor.Generic6DofJoint3D.NodeB = _playerPropInteractor.Generic6DofJoint3D.GetPath();
    }

    public void Drag(double delta)
    {
        _playerPropInteractor.CurrentObjectPosition = _playerPropInteractor.DraggedObject.GlobalTransform.Origin;
        Vector3 targetPosition = _playerPropInteractor.AnchorForDraggedObject.GlobalTransform.Origin;
        Vector3 newPosition = _playerPropInteractor.CurrentObjectPosition.Lerp(targetPosition, _playerPropInteractor.DragSpeed * (float)delta);

        Transform3D newTransform = _playerPropInteractor.DraggedObject.GlobalTransform;
        newTransform.Origin = newPosition;
        _playerPropInteractor.DraggedObject.GlobalTransform = newTransform;

        _playerPropInteractor.PreviousFrameObjectPosition = _playerPropInteractor.CurrentObjectPosition;
    }
}
