using Godot;

public partial class PlayerPropInteractor : Node
{
    [Export] private InputActions _inputActions;
    [Export] public RayCast3D RayOfLook;
    [Export] public CharacterBody3D PlayerBody;
    [Export] public Marker3D AnchorForDraggedObject;
    [Export] public StaticBody3D StaticBody3D;
    [Export] public Generic6DofJoint3D Generic6DofJoint3D;

    [ExportGroup("Parameters")]
    [Export] public float DragSpeed = 25.0f;
    [Export] public float RotationSpeed = 0.5f;
    [Export] public float ThrowForce = 8.0f;
    [Export] public float MaxDropInertia = 10f;
    [Export] public float DropInertiaModifier = 1f;

    public PlayerPropDrag PlayerPropDrag;
    public PlayerPropRotate PlayerPropRotate;
    public PlayerPropThrow PlayerPropThrow;

    public RigidBody3D DraggedObject;
    public Vector3 CurrentObjectPosition;
    public Vector3 PreviousFrameObjectPosition;

    public override void _Ready()
    {
        InitializationFields();
        _inputActions.InteractKeyDown += PlayerPropDrag.OnInteractKeyPressed;
        _inputActions.AlternativeKeyDown += PlayerPropRotate.StartRotating;
        _inputActions.AlternativeKeyUp += PlayerPropRotate.StopRotating;
        _inputActions.AttackKeyDown += PlayerPropThrow.Throw;
    }

    public override void _ExitTree()
    {
        _inputActions.InteractKeyDown -= PlayerPropDrag.OnInteractKeyPressed;
        _inputActions.AlternativeKeyDown -= PlayerPropRotate.StartRotating;
        _inputActions.AlternativeKeyUp -= PlayerPropRotate.StopRotating;
        _inputActions.AttackKeyDown -= PlayerPropThrow.Throw;
    }

    private void InitializationFields()
    {
        PlayerPropDrag = new PlayerPropDrag(this);
        PlayerPropRotate = new PlayerPropRotate(this);
        PlayerPropThrow = new PlayerPropThrow(this);
    }

    public Vector3 CalculateInertia()
    {
        CurrentObjectPosition = DraggedObject.GlobalTransform.Origin;
        Vector3 currentObjectVelocity = (CurrentObjectPosition - PreviousFrameObjectPosition) * DropInertiaModifier / (float)GetPhysicsProcessDeltaTime();
        if (currentObjectVelocity.Length() > MaxDropInertia)
            currentObjectVelocity = currentObjectVelocity.Normalized() * MaxDropInertia;

        return currentObjectVelocity;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (DraggedObject != null)
            PlayerPropDrag.Drag(delta);
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            PlayerPropRotate.Rotate(mouseMotion);
        }
    }
}
