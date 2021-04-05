using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class BGMusic : MonoBehaviour
{
    private FMOD.Studio.EventInstance music;

    public static BGMusic instance = null;

    void Awake() {
        if (instance == null) {
            instance = this;
            return;
        }

        Destroy(this);
    }

    public void ChangeTrack(string eventName) {
        music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        music = RuntimeManager.CreateInstance(eventName);
        music.start();
    }
}
