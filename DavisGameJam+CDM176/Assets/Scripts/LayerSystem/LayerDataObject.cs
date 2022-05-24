using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Layer Data")]
public class LayerDataObject : ScriptableObject
{
    public List<LayerInfo> layers;
}

[System.Serializable]
public class LayerInfo
{
    public string layerName = "New game layer";
    public Color layerColor = Color.white;
    public Texture2D tex;
}