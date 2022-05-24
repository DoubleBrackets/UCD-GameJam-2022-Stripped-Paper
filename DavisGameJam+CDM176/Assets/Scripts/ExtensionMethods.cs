using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static float Angle(this Vector2 vec)
    {
        return Mathf.Rad2Deg * Mathf.Atan2(vec.y, vec.x);
    }

    public static bool IsInLayerMask(this GameObject gameObject, LayerMask mask)
    {
        return ((mask.value) & (1 << gameObject.layer)) == (1 << gameObject.layer);
    }

    public static void BasicHorizontalMovement(
        this Rigidbody2D rb, 
        float targetSpeed, 
        float accelerationStep, 
        Vector2 normal, 
        float dir)
    {
        Vector2 relativeXAxis = (Vector2.right).ProjectOntoLineNormal(normal);
        float currentSpeed = Vector2.Dot(rb.velocity, relativeXAxis);
        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed * dir, accelerationStep);
        rb.velocity += relativeXAxis * (newSpeed - currentSpeed);
    }

    /// <summary>
    /// normal should be normalized
    /// </summary>
    /// <param name="toProject"></param>
    /// <param name="normal"></param>
    /// <returns></returns>
    public static Vector2 ProjectOntoLineNormal(this Vector2 toProject, Vector2 normal)
    {
        Vector2 projOnNorm = Vector2.Dot(toProject, normal) * normal;
        return toProject - projOnNorm;
    }


    public static Vector2 Rotate90(this Vector2 target,int dir = 1)
    {
        return new Vector3(dir * -target.y, dir * target.x);
    }
}
