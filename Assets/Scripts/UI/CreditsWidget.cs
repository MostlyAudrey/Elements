using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsWidget : MonoBehaviour
{
    public ScrollRect scrollView;
    public float autoScrollSpeed = 1f;

    void OnEnable()
    {
        //Scroll through credits
        LeanTween.value(gameObject, UpdateScroll, 1f, 0f, 1f / autoScrollSpeed);
    }

    private void UpdateScroll(float val)
    {
        scrollView.verticalScrollbar.value = val;
    }
}
