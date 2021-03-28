using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public enum SoundGroupName
{
    NONE,
    MASTER,
    MUSIC,
    DIALOGUE,
    SFX,
    UI
}

public class SoundGroup
{
    private string prefKey; //Playerpref key
    private string busPath;
    // Volume of sound group before making any changes
    private float initVolume;

    public SoundGroup(string _PlayerPrefKey, string _BusPath)
    {
        prefKey = _PlayerPrefKey;
        busPath = _BusPath;
        initVolume = AudioManager.getBusVolume(busPath);
        //Load volume multiplier from player preferences
        setVolumeMultiplier(getVolumeMultiplier());
    }

    public void setVolumeMultiplier(float multiplier)
    {
        //Save playerpref and multiply bus volume
        PlayerPrefs.SetFloat(prefKey, multiplier);
        AudioManager.setBusVolume(busPath, initVolume * multiplier);
    }

    public float getVolumeMultiplier()
    {
        return PlayerPrefs.GetFloat(prefKey, 1f);
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public Dictionary<SoundGroupName, SoundGroup> soundGroups;

    void Awake()
    {
        if (instance != null)
        {  
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            //Initialize soundGroups
            try
            {
                soundGroups = new Dictionary<SoundGroupName, SoundGroup>();
                soundGroups.Add(SoundGroupName.MASTER,      new SoundGroup("masterVol"  , ""            ));
                soundGroups.Add(SoundGroupName.MUSIC,       new SoundGroup("music"      , "Music"       ));
                soundGroups.Add(SoundGroupName.SFX,         new SoundGroup("sfx"        , "SFX"         ));
                soundGroups.Add(SoundGroupName.UI,          new SoundGroup("UI"         , "UI"          ));
            }
            catch (BusNotFoundException)
            {
                Debug.LogError("AudioManager: Sound groups set to buses that do not exist!");
            }
        }
    }

    public static float getBusVolume(string path)
    {
        FMOD.Studio.Bus bus = RuntimeManager.GetBus("bus:/" + path);
        float busVolume = 1f;
        bus.getVolume(out busVolume);
        return busVolume;
    }

    public static void setBusVolume(string path, float newVolume)
    {
        FMOD.Studio.Bus bus = RuntimeManager.GetBus("bus:/" + path);
        bus.setVolume(newVolume);
    }
}
