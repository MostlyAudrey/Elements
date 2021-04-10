using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWidget : MonoBehaviour
{
    public List<SettingsSlider> sliders;

    // SettingsSliders awake function may not have been called yet,
    // so call Reset in the onEnable of each SettingsSlider instead
    // void OnEnable()
    // {
    //     Reset();
    // }

    public void Reset()
    {
        foreach (SettingsSlider slider in sliders)
        {
            slider.Reset();
        }
    } 

    public void ApplyChanges()
    {
        foreach (SettingsSlider slider in sliders)
        {
            slider.ApplyChanges();
        }
    }
}
