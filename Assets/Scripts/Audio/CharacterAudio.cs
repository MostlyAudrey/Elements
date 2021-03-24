using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CharacterAudio : MonoBehaviour
{
    private FMOD.Studio.EventInstance swing;
    private FMOD.Studio.EventInstance step;

    public int surface;

    // Start is called before the first frame update
    void Start()
    {
        swing = RuntimeManager.CreateInstance("event:/Weapons/Swing");
        step = RuntimeManager.CreateInstance("event:/Character/Player Footsteps");
    }

    void Update() {
        
    }

    public void SwordSwing() {
        swing.start();
    }

    public void Footstep() {
        step.set3DAttributes(RuntimeUtils.To3DAttributes(transform.position));
        print(surface);
        step.setParameterByName("Surface", surface);
        FMOD.ATTRIBUTES_3D attributes;
        step.get3DAttributes(out attributes);
        step.start();
    }
}
