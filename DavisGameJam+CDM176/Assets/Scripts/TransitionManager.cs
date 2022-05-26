using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;
    private SpriteRenderer spriteRen;

    private Camera main;

    public UnityEvent transitionIn;
    public UnityEvent transitionOut;

    private void Awake()
    {
        instance = this;
        spriteRen = GetComponent<SpriteRenderer>();
        main = Camera.main;
        spriteRen.enabled = true;
        UpdateScale();
    }

    private void LateUpdate()
    {
        UpdateScale();
    }

    private void UpdateScale()
    {
        transform.localScale = new Vector3(2 * main.orthographicSize * Screen.width / Screen.height + 0.5f, 2 * main.orthographicSize + 0.5f, 1);
        MaterialPropertyBlock posBlock = new MaterialPropertyBlock();
        spriteRen.GetPropertyBlock(posBlock);
        posBlock.SetVector("_Scale", transform.lossyScale);
        posBlock.SetVector("_Position", transform.position);
        spriteRen.SetPropertyBlock(posBlock);
    }

    private Coroutine transitionCorout;

    public Coroutine TransitionIn(float time)
    {
        transitionIn?.Invoke();
        if (transitionCorout != null)
            StopCoroutine(transitionCorout);
        transitionCorout = StartCoroutine(StripAnimation(time));
        return transitionCorout;
    }

    private IEnumerator StripAnimation(float duration)
    {
        float timer = 0f;
        MaterialPropertyBlock posBlock = new MaterialPropertyBlock();
        spriteRen.GetPropertyBlock(posBlock);
        while (timer <= duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;
            float upperY = transform.position.y + main.orthographicSize + 6f;
            float lowerY = transform.position.y - main.orthographicSize - 3f;
            posBlock.SetFloat("_StripY", Mathf.Lerp(upperY, lowerY, t));
            spriteRen.SetPropertyBlock(posBlock);
            yield return new WaitForEndOfFrame();
        }
        posBlock.SetFloat("_StripY", -10000);
        spriteRen.SetPropertyBlock(posBlock);
        spriteRen.enabled = false;
    }

    public Coroutine TransitionOut(float time)
    {
        transitionOut?.Invoke();
        if (transitionCorout != null)
            StopCoroutine(transitionCorout);
        transitionCorout = StartCoroutine(UnstripAnimation(time));
        return transitionCorout;
    }

    private IEnumerator UnstripAnimation(float duration)
    {
        float timer = 0f;
        MaterialPropertyBlock posBlock = new MaterialPropertyBlock();
        spriteRen.GetPropertyBlock(posBlock);
        spriteRen.enabled = true;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Min(1, 1 - (timer / duration));
            float upperY = transform.position.y + main.orthographicSize + 3f;
            float lowerY = transform.position.y - main.orthographicSize - 3f;
            posBlock.SetFloat("_StripY", Mathf.Lerp(upperY, lowerY, t));
            spriteRen.SetPropertyBlock(posBlock);
            yield return new WaitForEndOfFrame();
        }
        posBlock.SetFloat("_StripY", 10000);
        spriteRen.SetPropertyBlock(posBlock);
    }
}
