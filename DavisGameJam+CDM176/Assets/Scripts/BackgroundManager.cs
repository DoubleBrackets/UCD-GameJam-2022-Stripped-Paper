using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [System.Serializable]
    public struct background
    {
        public Texture tex;
        public SpriteRenderer ren;
        public float scrollSpeed;
        public float scale;
        public float alpha;
        public float alphaPower;
    }
    public List<background> renderers;

    private void Awake()
    {
        UpdateMat();
    }

    private void Start()
    {
        LevelManager.instance.OnScreenTransition += Randomize;
    }

    private void OnValidate()
    {
        UpdateMat();
    }

    private void UpdateMat()
    {
        int i = 0;
        foreach (var bg in renderers)
        {
            var renderer = bg.ren;
            renderer.sortingOrder = i;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetVector("_Position", transform.position);
            block.SetTexture("_Texture", bg.tex);
            block.SetFloat("_ScrollSpeed", bg.scrollSpeed / bg.scale + Rand(0.1f));
            block.SetVector("_Scale", transform.lossyScale);
            block.SetFloat("_TexScale", bg.scale + Rand(0.25f));
            block.SetFloat("_Alpha", bg.alpha);
            block.SetFloat("_AlphaPower", bg.alphaPower + Rand(0.25f));
            renderer.SetPropertyBlock(block);
            i++;
        }
    }

    private void Randomize()
    {
        foreach (var bg in renderers)
        {
            var renderer = bg.ren;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetVector("_Offset", new Vector2(Rand(1f),Rand(1f)));
            renderer.SetPropertyBlock(block);
        }
        UpdateMat();
    }

    private float Rand(float val)
    {
        return Random.Range(-val, val);
    }

    private void Update()
    {
        foreach(var bg in renderers)
        {
            var renderer = bg.ren;
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            block.SetVector("_Position", transform.position);
            renderer.SetPropertyBlock(block);
        }
    }
}
