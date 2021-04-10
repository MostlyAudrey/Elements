using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeSlider : SettingsSlider
{
    public SoundGroupName soundGroup;

    public override void SetValue(float volumeMultiplier)
    {
        // Maximum volume multiplier for sound group is 2, minimum is 0
        slider.normalizedValue = volumeMultiplier * 0.5f;
    }

    public override float GetValue()
    {
        return 2f * slider.normalizedValue;
    }

    public override void Reset()
    {
        SetValue(AudioManager.instance.soundGroups[soundGroup].getVolumeMultiplier());
    }

    public override void ApplyChanges()
    {
        AudioManager.instance.soundGroups[soundGroup].setVolumeMultiplier(GetValue());
    }
}
