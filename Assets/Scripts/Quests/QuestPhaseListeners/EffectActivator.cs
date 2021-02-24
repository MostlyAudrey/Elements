using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectActivator : QuestPhaseListener
{
    public GameObject effect;
    public override void _action()
    {
        if (effect) effect.GetComponent<ParticleSystem>().Play();
        else Debug.Log("Effect is missing");
    }
}