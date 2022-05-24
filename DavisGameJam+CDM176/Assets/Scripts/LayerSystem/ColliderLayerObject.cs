using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLayerObject : BaseLayerObject
{
    protected Collider2D objectCollider;
    private float delay = 0.8f;

    protected override void Start()
    {
        base.Start();
        objectCollider = GetComponentInChildren<Collider2D>();
    }

    protected override void OnStripped()
    {
        StartCoroutine(RemoveCollider());
    }

    protected override void OnUnstripped()
    {
        StartCoroutine(SuffocationKill());
    }

    private IEnumerator RemoveCollider()
    {
        yield return new WaitForSeconds(delay);
        objectCollider.enabled = false;
    }

    private IEnumerator SuffocationKill()
    {
        yield return new WaitForSeconds(delay/2f);
        objectCollider.enabled = true;
        yield return new WaitForSeconds(delay / 2f);
        Collider2D[] hits = new Collider2D[10];
        var filter = new ContactFilter2D();
        filter.NoFilter();
        int hitCount = objectCollider.OverlapCollider(filter, hits);
        for (int i = 0; i < hitCount; i++)
        {
            var player = hits[i].gameObject.GetComponentInParent<PlayerStateController>();
            if (player != null)
            {
                player.KillPlayer();
            }
        }
    }

}
