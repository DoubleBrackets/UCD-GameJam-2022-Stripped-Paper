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

    private int currentTopLayer;

    public int CurrentTopLayer => currentTopLayer;

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

    [ContextMenu("Strip top layer")]
    public bool StripTopLayer()
    {
        if (currentTopLayer > layerEvents.Count - 1)
            return false;
        StripLayer(currentTopLayer);
        currentTopLayer++;
        if (!HasListeners(currentTopLayer-1))
            return StripTopLayer();
        return true;
    }

    [ContextMenu("Unstrip top layer")]
    public bool UnstripTopLayer()
    {
        if (currentTopLayer <= 0)
            return false;
        UnstripLayer(currentTopLayer - 1);
        currentTopLayer--;
        if (!HasListeners(currentTopLayer))
            return UnstripTopLayer();
        return true;
    }

    [ContextMenu("Reset")]
    public void ResetLayerManager()
    {
        currentTopLayer = 0;
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