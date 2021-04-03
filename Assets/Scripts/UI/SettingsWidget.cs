using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Wrapper class for volume multiplier sliders exposed in the UI.
 */
[System.Serializable]
public class VolumeSlider
{
    public Slider slider;
    public SoundGroupName soundGroup;

    public void SetValue(float volumeMultiplier)
    {
        slider.normalizedValue = volumeMultiplier * 0.5f;
    }

    public float GetValue()
    {
        //Maximum volume multiplier for sound group is twice the volume, minimum is 0
        return 2f * slider.normalizedValue;
    }

    public void ResetValue()
    {
        SetValue(AudioManager.instance.soundGroups[soundGroup].getVolumeMultiplier());
    }

    public void ApplyValue()
    {
        AudioManager.instance.soundGroups[soundGroup].setVolumeMultiplier(GetValue());
    }
}

public class SettingsWidget : MonoBehaviour
{
    public List<VolumeSlider> volumeSliders;

    void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        foreach (VolumeSlider slider in volumeSliders)
        {
            slider.ResetValue();
        }
    } 

    public void ApplyChanges()
    {
        foreach (VolumeSlider slider in volumeSliders)
        {
            slider.ApplyValue();
        }
    }
}
