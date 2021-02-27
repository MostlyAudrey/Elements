using System;
using System.Collections.Generic;
using UnityEngine;

public class EffectDeactivator : QuestPhaseListener
{
    public GameObject effect;
    public override void _action()
    {
        if (effect) effect.GetComponent<ParticleSystem>().Stop();
        else Debug.Log("Effect is missing");
    }
}