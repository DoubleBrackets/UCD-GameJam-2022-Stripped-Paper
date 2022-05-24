using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAbstractState
{
    protected PlayerStateController context;
    public PlayerAbstractState(PlayerStateController controller)
    {
        this.context = controller;
    }

    public abstract void EnterState();
    public abstract void ExitState();
    public abstract void UpdateState();
    public abstract void FixedUpdateState();
    public virtual void SwitchState(PlayerAbstractState newState)
    {
        ExitState();
        context.CurrentState = newState;
        newState.EnterState();
    }

    // Some util shit

    protected virtual void RotateAlongGround()
    {
        RotateAlongGround(context.groundedMovement.GroundedNormal);
    }

    protected virtual void RotateAlongGround(Vector2 normal)
    {
        float rawGroundAngle = normal.Angle() - 90f;
        float targetRotation = rawGroundAngle * context.movementProfile.rotateToGroundRatio;
        float rotateBounds = context.movementProfile.maxPlayerRotation;
        targetRotation = Mathf.Clamp(targetRotation, -rotateBounds, rotateBounds);
        float finalRotation = Mathf.MoveTowardsAngle(context.transform.eulerAngles.z, targetRotation, context.movementProfile.rotateSpeed * Time.deltaTime);
        context.transform.eulerAngles = new Vector3(
            0f,
            0f,
            finalRotation
        );
    }

}
