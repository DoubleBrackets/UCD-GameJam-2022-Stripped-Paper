using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLayerObject : BaseLayerObject
{
    protected SpriteRenderer spriteRen;
    public Vector3 upper;
    public Vector3 dissolve;
    private float duration = 1f;
    private bool isInTransition = false;
    private float curveCenter;
    public bool rotateWhenFlipping = false;

    MaterialPropertyBlock propertyBlock;

    protected override void Start()
    {
        base.Start();
        spriteRen = GetComponentInChildren<SpriteRenderer>();
        spriteRen.sortingOrder = -layerIndex;
        UpdateMaterial();
        spriteRen.color = layerColor;
    }

    private void UpdateMaterial()
    {
        MaterialPropertyBlock posBlock = new MaterialPropertyBlock();
        spriteRen.GetPropertyBlock(posBlock);
        posBlock.SetVector("_Scale", transform.lossyScale);
        posBlock.SetVector("_Position", transform.position);
        posBlock.SetVector("_OverlayOffset", new Vector2(Random.Range(0f,1f),Random.Range(0f,1f)));
        posBlock.SetFloat("_OverlayRotation", Random.Range(0,Mathf.PI*2));
        posBlock.SetFloat("_VaporizeThreshhold", transform.position.y + dissolve.y);
        posBlock.SetTexture ("_SecondaryTex", layerTexture);
        curveCenter = posBlock.GetFloat("_CurveCenter");
        spriteRen.SetPropertyBlock(posBlock);
    }

    public void UpdatePositions()
    {
        MaterialPropertyBlock posBlock = new MaterialPropertyBlock();
        spriteRen.GetPropertyBlock(posBlock);
        posBlock.SetFloat("_VaporizeThreshhold", transform.position.y + dissolve.y);
        if(!isInTransition) posBlock.SetFloat("_StripY", transform.position.y + upper.y);
        posBlock.SetVector("_Scale", transform.lossyScale);
        posBlock.SetVector("_Position", transform.position);
        spriteRen.SetPropertyBlock(posBlock);
    }

    private void Update()
    {
        UpdatePositions();
    }

    public override void OnValidate()
    {
        layerIndex =  GetComponent<LayerSpecifier>().layer;
        spriteRen = GetComponentInChildren<SpriteRenderer>();
        //spriteRen.color = Resources.Load<LayerDataObject>("ScriptableObjects/LayerData").layers[layerIndex].layerColor;
    }

    private Coroutine corout;

    protected override void OnStripped()
    {
        if (corout != null) StopCoroutine(corout);
        if(rotateWhenFlipping)
            transform.rotation = Quaternion.identity;
        corout = StartCoroutine(StripAnimation());
    }

    private IEnumerator StripAnimation()
    {
        isInTransition = true;
        float timer = 0f;
        MaterialPropertyBlock posBlock = new MaterialPropertyBlock();
        spriteRen.GetPropertyBlock(posBlock);
        while (timer <= duration)
        {
            timer += Time.fixedDeltaTime;
            float t = timer / duration;
            float upperY = transform.position.y + upper.y;
            float lowerY = transform.position.y + dissolve.y - curveCenter;
            posBlock.SetFloat("_StripY", Mathf.Lerp(upperY,lowerY,t));
            spriteRen.SetPropertyBlock(posBlock);
            yield return new WaitForFixedUpdate();
        }
        spriteRen.enabled = false;
        isInTransition = false;
    }

    protected override void OnUnstripped()
    {
        if (corout != null) StopCoroutine(corout);
        if (rotateWhenFlipping)
            transform.rotation = Quaternion.identity;
        corout = StartCoroutine(UnstripAnimation());
    }

    private IEnumerator UnstripAnimation()
    {
        isInTransition = true;
        float timer = 0f;
        MaterialPropertyBlock posBlock = new MaterialPropertyBlock();
        spriteRen.GetPropertyBlock(posBlock);
        spriteRen.enabled = true;
        while (timer < duration)
        {
            timer += Time.fixedDeltaTime;
            float t = Mathf.Min(1,1 - (timer / duration));
            float upperY = transform.position.y + upper.y;
            float lowerY = transform.position.y + dissolve.y - curveCenter;
            posBlock.SetFloat("_StripY", Mathf.Lerp(upperY, lowerY, t));
            spriteRen.SetPropertyBlock(posBlock);
            yield return new WaitForFixedUpdate();
        }
        isInTransition = false;
    }
}
