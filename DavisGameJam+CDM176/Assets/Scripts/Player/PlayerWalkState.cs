using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerGroundedState
{
    public PlayerWalkState(PlayerStateController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        context.animator.Play("Walk");
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FixedUpdateState()
    {
        RotateAlongGround();
        context.rb.BasicHorizontalMovement(
            context.movementProfile.movementSpeed,
            context.movementProfile.groundedAccel * Time.fixedDeltaTime,
            context.groundedMovement.GroundedNormal,
            context.HorizontalInput);
        CheckForGrounded();
    }

    public override void UpdateState()
    {
        if(context.HorizontalInput == 0f)
        {
            SwitchState(new PlayerStoppingState(context));
        }
        if(context.rb.velocity.magnitude < 0.1f)
            context.animator.Play("Idle");
        else
            context.animator.Play("Walk");
    }
}

