using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerSpecifier : MonoBehaviour
{
    public int layer;

    public void OnValidate()
    {
        //layer = Mathf.Clamp(layer, 0, Resources.Load<LayerDataObject>("ScriptableObjects/LayerData").layers.Count - 1);
        var hits = GetComponents<BaseLayerObject>();
        foreach (var layer in hits)
            layer.OnValidate();
    }
}
