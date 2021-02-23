using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public GameObject ContainerRect;
    public GameObject FillRect;

    void Start()
    {
    }

    void Update()
    {
        // For testing
        //SetPercentFill(Time.unscaledTime % 1f);
    }

    // Set amount of fill with float between 0 and 1
    public void SetPercentFill(float percent)
    {
        RectTransform m_ContainerRect = ContainerRect.transform as RectTransform;
        RectTransform m_FillRect = FillRect.transform as RectTransform;
        // Change fill rect's sizeDelta (does not update when changing rect.width or rect)
        m_FillRect.sizeDelta = new Vector2(m_ContainerRect.sizeDelta.x * percent, m_FillRect.sizeDelta.y);
        Debug.Log("Set percent fill to " + percent + " at width " + m_ContainerRect.sizeDelta.x * percent);
    }
}
