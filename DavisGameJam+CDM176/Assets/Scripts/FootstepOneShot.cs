using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FootstepOneShot: MonoBehaviour
{
    public GroundedMovement groundedMovement;

    public EventReference defaultStep;
    public EventReference paperStep;
    public LayerMask paperMask;
    public void PlayOneShot()
    {
        if(groundedMovement.floor.IsInLayerMask(paperMask))
            FMODUnity.RuntimeManager.PlayOneShot(paperStep, transform.position);
        else
            FMODUnity.RuntimeManager.PlayOneShot(defaultStep,transform.position);
    }
}
