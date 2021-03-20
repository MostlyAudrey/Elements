using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWidget : MonoBehaviour
{
    public Slider masterVolSlider;
    public Slider musicSlider;
    public Slider dialogueSlider;
    public Slider sfxSlider;

    void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        _setVolMultiplierInSlider(masterVolSlider,  AudioManager.instance.getSoundGroupVolMultiplier(SoundGroupName.MASTER));
        _setVolMultiplierInSlider(musicSlider,      AudioManager.instance.getSoundGroupVolMultiplier(SoundGroupName.MUSIC));
        _setVolMultiplierInSlider(dialogueSlider,   AudioManager.instance.getSoundGroupVolMultiplier(SoundGroupName.DIALOGUE));
        _setVolMultiplierInSlider(sfxSlider,        AudioManager.instance.getSoundGroupVolMultiplier(SoundGroupName.SFX));
    } 

    public void ApplyChanges()
    {
        float masterVolMultiplier = _getVolMultiplierFromSlider(masterVolSlider);
        float musicMultiplier = _getVolMultiplierFromSlider(musicSlider);
        float dialogueMultiplier = _getVolMultiplierFromSlider(dialogueSlider);
        float sfxMultiplier = _getVolMultiplierFromSlider(sfxSlider);

        AudioManager.instance.setSoundGroupVolMultiplier(SoundGroupName.MASTER  , masterVolMultiplier);
        AudioManager.instance.setSoundGroupVolMultiplier(SoundGroupName.MUSIC   , musicMultiplier);
        AudioManager.instance.setSoundGroupVolMultiplier(SoundGroupName.DIALOGUE, dialogueMultiplier);
        AudioManager.instance.setSoundGroupVolMultiplier(SoundGroupName.SFX     , sfxMultiplier);
    }  

    private float _getVolMultiplierFromSlider(Slider slider)
    {
        //Maximum multiplier is twice the volume, minimum is 0
        return 2f * slider.normalizedValue;
    }

    private void _setVolMultiplierInSlider(Slider slider, float multiplier)
    {
        slider.normalizedValue = multiplier * 0.5f;
    }
}
