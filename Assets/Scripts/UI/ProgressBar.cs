using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    [Tooltip("Transform must only have anchors and no size delta.")]
    public GameObject containerRect;
    public GameObject fillRect;


    void Update()
    {
        // For testing
        //SetPercentFill(Time.unscaledTime % 1f);
    }

    // Set amount of fill with float between 0 and 1
    public void SetPercentFill(float percent)
    {
        RectTransform containerTransform = containerRect.transform as RectTransform;
        RectTransform fillTransform = fillRect.transform as RectTransform;

        fillTransform.sizeDelta = containerTransform.sizeDelta;
        fillTransform.pivot = containerTransform.pivot;
        fillTransform.anchorMin = containerTransform.anchorMin;

        //PercentFill*(anchorMax.x-anchorMin.x)+anchorMin.x = PercentFill*anchorMax.X+(1-PercentFill)*anchorMin.X  
        fillTransform.anchorMax = new Vector2(percent * containerTransform.anchorMax.x + (1f - percent) * containerTransform.anchorMin.x,
            containerTransform.anchorMax.y);

        //FillTransform.sizeDelta = new Vector2(ContainerTransform.sizeDelta.x * percent, ContainerTransform.sizeDelta.y);
        //Debug.Log("Set percent fill to " + percent + " at width " + ContainerTransform.sizeDelta.x * percent);
    }
}
