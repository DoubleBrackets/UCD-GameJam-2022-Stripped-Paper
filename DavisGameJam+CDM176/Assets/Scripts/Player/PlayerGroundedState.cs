using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerGroundedState : PlayerAbstractState
{
    protected PlayerGroundedState(PlayerStateController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        context.OnJumpPressed += Jump;
    }

    public override void ExitState()
    {
        context.OnJumpPressed -= Jump;   
    }

    protected virtual void CheckForGrounded()
    {
        if(!context.groundedMovement.IsGrounded)
        {
            SwitchState(new PlayerAirborneState(context));
        }
    }

    protected virtual void Jump()
    {
        SwitchState(new PlayerJumpState(context));
    }
}
