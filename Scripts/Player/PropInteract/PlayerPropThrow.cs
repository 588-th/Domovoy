using Godot;

public partial class PlayerPropThrow
{
    private PlayerPropInteractor _playerPropInteractor;

    public PlayerPropThrow(PlayerPropInteractor playerPropInteractor)
    {
        _playerPropInteractor = playerPropInteractor;
    }

    public void Throw()
    {
        if (_playerPropInteractor.DraggedObject == null)
            return;

        RigidBody3D draggedObject = _playerPropInteractor.DraggedObject;

        Vector3 lookDirection = -_playerPropInteractor.RayOfLook.GlobalTransform.Basis.Z;
        Vector3 throwImpulse = lookDirection * _playerPropInteractor.ThrowForce;
        Vector3 currentObjectVelocity = _playerPropInteractor.CalculateInertia();

        _playerPropInteractor.PlayerPropDrag.StopDragging();
        _playerPropInteractor.PlayerPropRotate.StopRotating();

        draggedObject.AngularVelocity = Vector3.Zero;
        draggedObject.LinearVelocity = Vector3.Zero;
        draggedObject.ApplyCentralImpulse(throwImpulse + currentObjectVelocity);
    }
}
