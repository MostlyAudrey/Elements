using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsQualitySlider : SettingsSlider
{
    public int minQualityLevel = 1;
    public int maxQualityLevel = 5;

    public override void SetValue(float qualityLevel)
    {
        // Map range from minQualityLevel to maxQualityLevel onto [0, 1]
        // Let normalizedValue property handle clamping
        slider.normalizedValue = (qualityLevel - minQualityLevel) / (maxQualityLevel - minQualityLevel);
    }

    public override float GetValue()
    {
        // Map [0, 1]  to a whole number from minQualityLevel to maxQualityLevel
        return Mathf.Round(slider.normalizedValue * (maxQualityLevel - minQualityLevel) + minQualityLevel);
    }

    public override void Reset()
    {
        SetValue(QualitySettings.GetQualityLevel());
    }

    public override void ApplyChanges()
    {
        QualitySettings.SetQualityLevel((int) GetValue(), true);
    }
}
