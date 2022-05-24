using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public static TerrainManager instance;
    public LayerMask staticTerrainMask;
    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("Terrain Setup")]
    public void SetupStaticTerrain()
    {
        var gameObjects = FindObjectsOfType<SpriteRenderer>();
        foreach(var rend in gameObjects)
        {
            if(rend.gameObject.IsInLayerMask(staticTerrainMask))
            {
                MaterialPropertyBlock block = new MaterialPropertyBlock();
                rend.GetPropertyBlock(block);
                block.SetVector("_Scale", rend.transform.lossyScale);
                block.SetVector("_OFfset", new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
                rend.SetPropertyBlock(block);
            }
        }
    }
}
