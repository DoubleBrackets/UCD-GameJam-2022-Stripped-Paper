using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPortalState : PlayerAbstractState
{
    public PlayerPortalState(PlayerStateController controller) : base(controller)
    {
    }

    private float timer = 0f;
    public override void EnterState()
    {
        context.animator.Play("Portal");
        context.CanSwitchLayer = false;
        context.rb.constraints = RigidbodyConstraints2D.FreezeAll;
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

    }
}
