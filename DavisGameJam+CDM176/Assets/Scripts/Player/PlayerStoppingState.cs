using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStoppingState : PlayerGroundedState
{
    public PlayerStoppingState(PlayerStateController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        context.animator.Play("Stopping");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdateState()
    {
        RotateAlongGround(Vector2.up);
        context.rb.BasicHorizontalMovement(
            0f,
            context.movementProfile.groundedFrictionAccel * Time.fixedDeltaTime,
            context.groundedMovement.GroundedNormal,
            context.HorizontalInput);
        CheckForGrounded();
    }

    public override void UpdateState()
    {
        if(context.rb.velocity.magnitude <= 0.4f)
        {
            SwitchState(new PlayerIdleState(context));
        }
        else if(context.HorizontalInput != 0)
        {
            SwitchState(new PlayerWalkState(context));
        }
    }
}

