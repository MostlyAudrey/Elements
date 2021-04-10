using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image mask;

    [SerializeField, Range(0f,1f)]
    private float percentFill;

    void OnValidate()
    {
        SetPercentFill(percentFill);
    }

    public void SetPercentFill(float percent)
    {
        percentFill = percent;
        mask.fillAmount = percent;
    }
}
