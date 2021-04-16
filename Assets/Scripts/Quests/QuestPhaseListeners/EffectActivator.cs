using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectActivator : QuestPhaseListener
{
    public GameObject effect;
    public bool emit_only = false;

    public void Start()
    {
        effect.GetComponent<ParticleSystem>().Stop();
    }

    public override void _action()
    {
        if (effect)
        {
            if (!effect.GetComponent<ParticleSystem>().isPlaying)
            { 
                if(emit_only) effect.GetComponent<ParticleSystem>().loop = false;
                
                effect.GetComponent<ParticleSystem>().Play();
            }
            // var emission = effect.GetComponent<ParticleSystem>().emission;
            // emission.enabled = true;
        }
        else Debug.Log("Effect is missing");
    }
}