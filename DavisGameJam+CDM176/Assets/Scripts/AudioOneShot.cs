using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class AudioOneShot : MonoBehaviour
{
    public EventReference audioEvent;
    public void PlayOneShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(audioEvent);
    }
}
