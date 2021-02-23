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
        float masterVolume = _getBusVolume("");
        masterVolumeSlider.value = masterVolume;
        masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
    }

    void OnMasterVolumeChanged(float newValue)
    {
        _setBusVolume("", newValue);
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
