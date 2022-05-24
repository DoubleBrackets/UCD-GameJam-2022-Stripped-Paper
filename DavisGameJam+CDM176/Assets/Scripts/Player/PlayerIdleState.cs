using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    public PlayerIdleState(PlayerStateController controller) : base(controller)
    {
    }


    public override void EnterState()
    {
        base.EnterState();
        context.animator.Play("Idle");
        context.rb.constraints |= RigidbodyConstraints2D.FreezePositionX;
    }

    public override void ExitState()
    {
        base.ExitState();
        context.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void FixedUpdateState()
    {
        RotateAlongGround(Vector2.up);
        CheckForGrounded();
    }

    public override void UpdateState()
    {
        if(context.HorizontalInput != 0f)
        {
            SwitchState(new PlayerWalkState(context));
        }
    }
}
