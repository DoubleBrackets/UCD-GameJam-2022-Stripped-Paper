using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class PlayerAudioManager : MonoBehaviour
{
    public EventReference footStep;
    public EventReference jump;
    public EventReference strip;
    public EventReference unstrip;

    public void PlayOneShot(EventReference fmodEvent)
    {
        FMODUnity.RuntimeManager.PlayOneShot(fmodEvent,transform.position);
    }
}
