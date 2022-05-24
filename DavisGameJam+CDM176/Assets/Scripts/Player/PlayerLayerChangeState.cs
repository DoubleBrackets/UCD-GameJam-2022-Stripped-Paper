using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLayerChangeState : PlayerAbstractState
{
    public PlayerLayerChangeState(PlayerStateController controller) : base(controller)
    {
    }

    private float timer;

    public override void EnterState()
    {
        context.animator.Play("Cast");
        context.CanSwitchLayer = false;
        context.rb.constraints = RigidbodyConstraints2D.FreezeAll;
        timer = context.movementProfile.layerSwitchDuration;
    }

    public override void ExitState()
    {
        context.CanSwitchLayer = true;
        context.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public override void FixedUpdateState()
    {

    }

    public override void UpdateState()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (context.groundedMovement.IsGrounded)
            {
                SwitchState(new PlayerIdleState(context));
            }
            else
            {
                SwitchState(new PlayerAirborneState(context));
            }
        }
    }
}
