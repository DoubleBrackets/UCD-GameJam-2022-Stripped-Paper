using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
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
}
