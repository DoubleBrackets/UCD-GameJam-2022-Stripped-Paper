using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerIndicatorGenerator : MonoBehaviour
{
    public GameObject prefab;

    private List<GameObject> indicators = new List<GameObject>();
    public void Regenerate()
    {
        foreach(GameObject indicator in indicators)
        {
            Destroy(indicator);
        }
        indicators.Clear();
        var activeLayers = GameLayerManager.instance.GetPopulatedLayers();
        for(int i = 0;i < activeLayers.Count;i++)
        {
            if(activeLayers[i])
            {
                var indicator = Instantiate(prefab, transform.position, Quaternion.identity, transform);
                indicator.GetComponent<LayerSpecifier>().layer = i;
                indicator.GetComponent<SpriteRenderer>().sortingOrder = -i;
                indicators.Add(indicator);
            }
        }
    }
}
