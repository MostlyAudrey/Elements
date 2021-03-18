using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class SettingsDisplay : MonoBehaviour
{
    [Tooltip("Slider component for master volume")]
    public Slider masterVolumeSlider;

    void Start()
    {
        //float masterVolume = _getBusVolume("");
        //masterVolumeSlider.value = masterVolume;
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
    }

    void OnMasterVolumeChanged(float newValue)
    {
        float masterVolume = _getBusVolume("");
        //Middle of slider corresponds to original master volume, right end corresponds to twice the volume, left end corresponds to 0
        masterVolume *= 2f * (newValue - masterVolumeSlider.minValue) / (masterVolumeSlider.maxValue - masterVolumeSlider.minValue);
        _setBusVolume("", masterVolume);
    }

    private float _getBusVolume(string path)
    {
        FMOD.Studio.Bus bus = RuntimeManager.GetBus("bus:/" + path);
        float busVolume = 1f;
        bus.getVolume(out busVolume);
        return busVolume;
    }

    private void _setBusVolume(string path, float newVolume)
    {
        FMOD.Studio.Bus bus = RuntimeManager.GetBus("bus:/" + path);
        bus.setVolume(newVolume);
    }
}
