using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerAbstractState
{
    public PlayerDeadState(PlayerStateController controller) : base(controller)
    {
    }

    private float timer = 0f;
    public override void EnterState()
    {
        context.animator.Play("Death");
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
        timer += Time.deltaTime;
        if (timer >= 2f)
            LevelManager.instance.RestartLevel();
    }
}
