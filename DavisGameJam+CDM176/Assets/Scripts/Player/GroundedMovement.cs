using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedMovement : MonoBehaviour
{
    public LayerMask groundedMask;
    public MovementProfile profile;
    public Rigidbody2D rb;

    public Transform castReference;

    private Vector2 groundedNormal;
    public Vector2 GroundedNormal {get => groundedNormal;}

    [SerializeField] private bool isGrounded;
    public bool IsGrounded {get => isGrounded;}

    private bool isPinned;
    public bool IsPinned { get => isPinned; }

    private int stepsSinceLastGrounded = 0;
    private Vector2 lastGroundedNormal = Vector2.up;
    
    public float snapToGroundBlock
    {
        get;
        set;
    }


    private void FixedUpdate()
    {
        GetGroundedNormal();
        UpdateGroundedState();
    }

    private void UpdateGroundedState()
    {
        snapToGroundBlock -= Time.deltaTime;
        if(IsGrounded || GroundedSnap())
        {
            isGrounded = true;
            stepsSinceLastGrounded = 0;
            lastGroundedNormal = groundedNormal;
        }
        else
        {
            stepsSinceLastGrounded++;
            groundedNormal = Vector2.up;
        }
    }

    private bool GroundedSnap()
    {
        if(snapToGroundBlock > 0f || stepsSinceLastGrounded > 0)
        {
            return false;
        }
        var hit = Physics2D.Raycast(
            castReference.transform.position,
            -Vector2.up,
            profile.groundedSnapDistance,
            groundedMask
            );

        if(hit.collider != null)
        {
            Vector2 vel = rb.velocity;
            if ((Vector2.Dot(hit.normal, vel.normalized.Rotate90((int)Mathf.Sign(vel.x))) >= profile.groundedSnapMaxDot)
                && (Vector2.Dot(Vector2.up, hit.normal) >= profile.minGroundedDot))
            {
                groundedNormal = hit.normal;
                if(Vector3.Dot(groundedNormal, vel) >= 0f)
                {
                    Vector2 adjustedVel = vel.ProjectOntoLineNormal(groundedNormal).normalized * vel.magnitude;
                    rb.velocity = adjustedVel;
                }
                return true;
            }
        }
        return false;
    }

    private void GetGroundedNormal()
    {
        var hits = Physics2D.CircleCastAll(
            castReference.transform.position + castReference.transform.up * profile.groundedCheckDistance,
            profile.groundedCheckDistance,
            - castReference.transform.up,
            profile.groundedCheckDistance,
            groundedMask
            );
        var norms = new List<Vector2>();
        foreach(var hit in hits)
        {
            if (hit.collider != null)
            {
                // Calculate hit normal
                float dot = Vector2.Dot(Vector2.up, hit.normal);
                bool isValid = (dot >= profile.minGroundedDot);
                if(isValid)
                    norms.Add(hit.normal);
            }
        }
        if(norms.Count == 0)
        {
            groundedNormal = Vector2.up;
            isGrounded = false;
            return;
        }
        isGrounded = true;
        groundedNormal = Vector2.zero;
        foreach (var norm in norms)
            groundedNormal += norm;
        groundedNormal /= norms.Count;        
    }

}
