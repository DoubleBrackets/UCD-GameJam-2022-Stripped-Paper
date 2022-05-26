using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
public class FMODParameterManager : MonoBehaviour
{
    public static FMODParameterManager instance;

    private void Awake()
    {
        instance = this;
    }

    public List<globalParam> paramList;

    public void SetParameter(string parameter, float val)
    {
        for(int i = 0;i < paramList.Count;i++)
        {
            if(paramList[i].name == parameter)
                paramList[i].targetValue = Mathf.Clamp01(val); 
        }    
    }

    private void Update()
    {
        foreach(var param in paramList)
        {
            param.value = Mathf.MoveTowards(param.value,param.targetValue,0.5f*Time.deltaTime);
            RuntimeManager.StudioSystem.setParameterByName(param.name, param.value);
        }
    }
}

[System.Serializable]
public class globalParam
{
    public string name;
    public float value;
    public float targetValue;
}
