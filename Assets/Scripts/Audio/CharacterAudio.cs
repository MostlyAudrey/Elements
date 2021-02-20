using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class CharacterAudio : MonoBehaviour
{
    FMOD.Studio.EventInstance swing;

    // Start is called before the first frame update
    void Start()
    {
        swing = RuntimeManager.CreateInstance("event:/Weapons/Swing");
    }

    public void SwordSwing() {
        swing.start();
    }
}
