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
    SFX
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    struct SoundGroupData
    {
        public string prefKey; //Playerpref key
        public string busPath;

        public SoundGroupData(string inPlayerPrefKey, string inBusPath)
        {
            prefKey = inPlayerPrefKey;
            busPath = inBusPath;
        }
    }

    private Dictionary<SoundGroupName, SoundGroupData> soundGroups;

    void Awake()
    {
        if (instance != null)
        {  
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            //Init dictionary
            soundGroups = new Dictionary<SoundGroupName, SoundGroupData>();
            soundGroups.Add(SoundGroupName.MASTER,      new SoundGroupData("masterVol"  , ""            ));
            soundGroups.Add(SoundGroupName.MUSIC,       new SoundGroupData("music"      , "music"       ));
            soundGroups.Add(SoundGroupName.DIALOGUE,    new SoundGroupData("dialogue"   , "dialogue"    ));
            soundGroups.Add(SoundGroupName.SFX,         new SoundGroupData("sfx"        , "sfx"         ));
        }
    }

    void Start()
    {
        //Load playerprefs
        setSoundGroupVolMultiplier(SoundGroupName.MASTER,   PlayerPrefs.GetFloat(soundGroups[SoundGroupName.MASTER].prefKey,    1f));
        setSoundGroupVolMultiplier(SoundGroupName.MUSIC,    PlayerPrefs.GetFloat(soundGroups[SoundGroupName.MUSIC].prefKey,     1f));
        setSoundGroupVolMultiplier(SoundGroupName.DIALOGUE, PlayerPrefs.GetFloat(soundGroups[SoundGroupName.DIALOGUE].prefKey,  1f));
        setSoundGroupVolMultiplier(SoundGroupName.SFX,      PlayerPrefs.GetFloat(soundGroups[SoundGroupName.SFX].prefKey,       1f));
    }

    public float getSoundGroupVolMultiplier(SoundGroupName soundGroupName)
    {
        return PlayerPrefs.GetFloat(soundGroups[soundGroupName].prefKey);
    }

    public void setSoundGroupVolMultiplier(SoundGroupName soundGroupName, float multiplier)
    {
        SoundGroupData soundGroupData = soundGroups[soundGroupName];
        //Save playerpref and multiply bus volume
        PlayerPrefs.SetFloat(soundGroupData.prefKey, multiplier);
        setBusVolume(soundGroupData.busPath, getBusVolume(soundGroupData.busPath) * multiplier);
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
