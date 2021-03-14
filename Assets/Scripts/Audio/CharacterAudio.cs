using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CharacterAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance swing;
    private FMOD.Studio.EventInstance step;

    // Start is called before the first frame update
    void Start()
    {
        swing = RuntimeManager.CreateInstance("event:/Weapons/Swing");
        step = RuntimeManager.CreateInstance("event:/Character/Player Footsteps");
        RuntimeManager.AttachInstanceToGameObject(step, transform, GetComponent<Rigidbody>());
    }

    public void SwordSwing() {
        swing.start();
    }

    public void Footstep() {
        step.start();
    }
}
