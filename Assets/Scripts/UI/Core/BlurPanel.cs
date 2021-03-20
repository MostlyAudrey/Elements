using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class BlurPanel : Image
{
    [Range(0, 4)]
    public float blurRadius = 1f;
    public bool animate = true;
    public float time = 0.5f;
    public float delay = 0f;

    CanvasGroup canvas;

    protected override void Awake()
    {
        base.Awake();

        canvas = GetComponent<CanvasGroup>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        if (animate && Application.isPlaying)
        {
            material.SetFloat("_Size", 0);
            canvas.alpha = 0;
            
            LeanTween.value(gameObject, UpdateBlur, 0f, blurRadius, time).setDelay(delay).setIgnoreTimeScale(true);
        }
        else
        {
            material.SetFloat("_Size", blurRadius);
            canvas.alpha = 1f;
        }
    }

    void UpdateBlur(float value)
    {
        material.SetFloat("_Size", value);
        canvas.alpha = value;
    }
}
