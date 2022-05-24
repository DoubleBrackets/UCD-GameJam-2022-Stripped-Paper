using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAbstractState
{
    public PlayerJumpState(PlayerStateController controller) : base(controller)
    {
    }

    public override void EnterState()
    {
        context.groundedMovement.snapToGroundBlock = 0.35f;
        context.rb.velocity = new Vector2(
            context.rb.velocity.x,
            context.movementProfile.jumpVel);
        SwitchState(new PlayerAirborneState(context));
    }

    public override void ExitState()
    {

    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {

    }
}
