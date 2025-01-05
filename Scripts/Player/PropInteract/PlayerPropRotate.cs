using Godot;

public partial class PlayerPropRotate
{
    private PlayerPropInteractor _playerPropInteractor;
    private bool _isRotating;

    public PlayerPropRotate(PlayerPropInteractor playerPropInteractor)
    {
        _playerPropInteractor = playerPropInteractor;
    }

    public void StartRotating()
    {
        _isRotating = true;
    }

    public void StopRotating()
    {
        _isRotating = false;
    }

    public void Rotate(InputEventMouseMotion mouseMotion)
    {
        if (!_isRotating || _playerPropInteractor.DraggedObject == null)
            return;

        float rotationSpeed = _playerPropInteractor.RotationSpeed;
        _playerPropInteractor.StaticBody3D.RotateX(Mathf.DegToRad(mouseMotion.Relative.Y * rotationSpeed));
        _playerPropInteractor.StaticBody3D.RotateY(Mathf.DegToRad(mouseMotion.Relative.X * rotationSpeed));
    }
}
