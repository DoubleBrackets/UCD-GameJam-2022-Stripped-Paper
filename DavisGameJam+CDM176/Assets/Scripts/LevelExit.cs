using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    private GameObject player;
    public float paramMaxDistance;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInParent<PlayerStateController>() != null)
        {
            var controller = collision.GetComponentInParent<PlayerStateController>();
            controller.CurrentState.SwitchState(new PlayerPortalState(controller));
            controller.transform.position = transform.position;
            LevelManager.instance.ProgressLevel();
        }
    }

    private void Awake()
    {
        player = FindObjectOfType<PlayerStateController>().gameObject;
    }


    private void Update()
    {
        float dist = (player.transform.position - transform.position).magnitude;
        FMODParameterManager.instance.SetParameter("Distance", Mathf.Min(1f,dist / paramMaxDistance));
    }
}
