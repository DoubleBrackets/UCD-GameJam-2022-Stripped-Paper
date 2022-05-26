using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameLayerManager : MonoBehaviour
{
    public static GameLayerManager instance;

    public LayerDataObject layerData;

    private List<LayerEventGroup> layerEvents = new List<LayerEventGroup>();
    public List<LayerEventGroup> LayerEvents
    {
        get => layerEvents;
        set => layerEvents = value;
    }

    [SerializeField]
    private int currentTopLayer;

    public int CurrentTopLayer
    {
        get => currentTopLayer;
        private set
        {
            currentTopLayer = value;
            int count = GetPopulatedLayerCount();
            FMODParameterManager.instance.SetParameter("Layers", GetLayerRatio(currentTopLayer));
        }
    }

    private void Awake()
    {
        instance = this;
        layerEvents.Clear();
        foreach (var data in layerData.layers)
        {
            layerEvents.Add(new LayerEventGroup());
        }
        ResetLayerManager();
    }

    private void StripLayer(int layerIndex)
    {
        layerEvents[layerIndex].InvokeStripLayer?.Invoke();
    }
    private void UnstripLayer(int layerIndex)
    {
        layerEvents[layerIndex].InvokeUnstripLayer?.Invoke();
    }

    public bool StripTopLayer()
    {
        if (currentTopLayer > layerEvents.Count - 1)
            return false;
        StripLayer(currentTopLayer);
        CurrentTopLayer++;
        if (!HasListeners(currentTopLayer-1))
        {
            bool found = StripTopLayer();
            if (!found)
                CurrentTopLayer--;
            return found;
        }
        return true;
    }

    public bool UnstripTopLayer()
    {
        if (currentTopLayer <= 0)
            return false;
        UnstripLayer(currentTopLayer - 1);
        CurrentTopLayer--;
        if (!HasListeners(currentTopLayer))
        {
            bool found = UnstripTopLayer();
            if (!found)
                CurrentTopLayer++;
            return found;
        }
        return true;
    }

    public void ResetLayerManager()
    {
        CurrentTopLayer = 0;
    }

    public LayerInfo GetLayerInfo(int index)
    {
        return layerData.layers[index];
    }

    private bool HasListeners(int index)
    {
        return layerEvents[index].InvokeStripLayer != null && layerEvents[index].InvokeStripLayer.GetInvocationList().Length > 0;
    }

    public List<bool> GetPopulatedLayers()
    {
        var results = new List<bool>();
        for(int i = 0;i < layerEvents.Count;i++)
        {
            results.Add(HasListeners(i));
        }
        return results;
    }

    public int GetPopulatedLayerCount()
    {
        int c = 0;
        for (int i = 0; i < layerEvents.Count; i++)
        {
            if (HasListeners(i))
                c++;
        }
        return c;
    }

    private float GetLayerRatio(int toplayer)
    {
        int count = 0;
        int currentCount = 0;
        for (int i = 0; i < layerEvents.Count; i++)
        {
            if(HasListeners(i))
            {
                if (toplayer > i)
                {
                    currentCount++;
                }
                count++;
            }
        }
        return count == 0 ? 0 : 1 - (currentCount / (float)count);
    }


    public void UnloadLayerObjects()
    {
        foreach(var layerEvent in layerEvents)
        {
            layerEvent.Unload?.Invoke();
        }
    }
}

public class LayerEventGroup
{
    public Action InvokeStripLayer;
    public Action InvokeUnstripLayer;
    public Action Unload;
}