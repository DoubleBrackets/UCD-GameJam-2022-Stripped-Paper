using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeAdjust : MonoBehaviour
{

    FMOD.Studio.EventInstance instance;
    FMOD.Studio.Bus bus;


    [SerializeField]
    [Range(-80f, 10f)]
    private float busVolume;
    private float volume;


    void Start()
    {
        bus = FMODUnity.RuntimeManager.GetBus("bus:/");
    }

    void Update()
    {
        volume = Mathf.Pow(10.0f, busVolume / 20f);
        bus.setVolume(volume);
    }
}