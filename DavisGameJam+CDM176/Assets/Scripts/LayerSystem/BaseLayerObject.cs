using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseLayerObject : MonoBehaviour
{
    protected int layerIndex;

    protected Color layerColor;
    protected Texture layerTexture;

    protected virtual void Start()
    {
        layerIndex = GetComponent<LayerSpecifier>().layer;
        layerColor = GameLayerManager.instance.GetLayerInfo(layerIndex).layerColor;
        layerTexture = GameLayerManager.instance.GetLayerInfo(layerIndex).tex;
        GameLayerManager.instance.LayerEvents[layerIndex].InvokeStripLayer += OnStripped;
        GameLayerManager.instance.LayerEvents[layerIndex].InvokeUnstripLayer += OnUnstripped;
        GameLayerManager.instance.LayerEvents[layerIndex].Unload += Unload;
    }

    private void Unload()
    {
        GameLayerManager.instance.LayerEvents[layerIndex].InvokeStripLayer -= OnStripped;
        GameLayerManager.instance.LayerEvents[layerIndex].InvokeUnstripLayer -= OnUnstripped;
        GameLayerManager.instance.LayerEvents[layerIndex].Unload -= Unload;
    }

    public virtual void OnValidate() { }

    protected abstract void OnStripped();

    protected abstract void OnUnstripped();
}
