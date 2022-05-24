using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAirborneState : PlayerAbstractState
{
    public PlayerAirborneState(PlayerStateController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        context.animator.Play("Airborne");
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdateState()
    {
        RotateAlongGround(Vector2.up);
        float accel = context.HorizontalInput == 0f ? context.movementProfile.airFriction : context.movementProfile.airAccel;

        context.rb.BasicHorizontalMovement(
            context.movementProfile.movementSpeed,
            accel * Time.fixedDeltaTime,
            Vector2.up,
            context.HorizontalInput);

        if (context.groundedMovement.IsGrounded)
        {
            if(context.HorizontalInput != 0)
            {
                SwitchState(new PlayerWalkState(context));
            }
            else
            {
                SwitchState(new PlayerIdleState(context));
            }
        }
    }

    public override void UpdateState()
    {
        if(Input.GetKeyDown(KeyCode.Space) && context.groundedMovement.IsPinned)
        {
            SwitchState(new PlayerJumpState(context));
        }
    }
}
