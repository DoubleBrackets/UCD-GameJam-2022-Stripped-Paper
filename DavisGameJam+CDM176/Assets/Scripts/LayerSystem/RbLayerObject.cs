using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RbLayerObject : BaseLayerObject
{
    protected Rigidbody2D rb;
    private float delay = 0.8f;

    private Vector2 vel;
    protected override void Start()
    {
        base.Start();
        rb = GetComponentInChildren<Rigidbody2D>();
    }

    protected override void OnStripped()
    {
        vel = rb.velocity;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    protected override void OnUnstripped()
    {
        StartCoroutine(EnableRb());
    }

    private IEnumerator EnableRb()
    {
        yield return new WaitForSeconds(delay);
        rb.constraints = RigidbodyConstraints2D.None;
        rb.velocity = vel;
    }

}
