using System;

public class MovementEventHandler
{


    public Action IsGrounded;
    public Action IsNotGrounded;
    public Action IsOnWall;
    public Action IsNotOnWall;
    public Action IsCeiling;

    public Action IsNotCeiling;
    public Action SlideButtonHold;
    public Action SlideButtonUp;
    public Action CrouchButtonHold;
    public Action CrouchButtonUp;
    public Action JumpButtonDown;

    public Action IsSlideTimeOut;
    public Action IsWallrunTimeOut;


}
