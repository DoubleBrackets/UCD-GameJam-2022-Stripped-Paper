using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MovementProfile")]
public class MovementProfile : ScriptableObject
{
    public float movementSpeed;
    public float rotateSpeed;

    [Space(15)]
    public float groundedAccel;
    public float groundedFrictionAccel;

    [Space(15)]
    public float airAccel;
    public float airFriction;

    [Space(15)]
    public float jumpHeight;

    public float rotateToGroundRatio;
    public float maxPlayerRotation;

    [Space(15)]
    public float layerSwitchDuration;

    [Space(15)]
    public float minGroundedDot;
    public float groundedCheckDistance;
    public float groundedSnapDistance;
    public float groundedSnapMaxAngle;

    public float jumpVel
    {
        get;
        private set;
    }

    public float groundedSnapMaxDot
    {
        get;
        private set;
    }

    private void OnValidate()
    {
        jumpVel = Mathf.Sqrt(2*-Physics2D.gravity.y*jumpHeight);
        groundedSnapMaxDot = Mathf.Cos(groundedSnapMaxAngle * Mathf.Deg2Rad);
    }

    private void OnEnable()
    {
        jumpVel = Mathf.Sqrt(2 * -Physics2D.gravity.y * jumpHeight);
        groundedSnapMaxDot = Mathf.Cos(groundedSnapMaxAngle * Mathf.Deg2Rad);
    }

}
