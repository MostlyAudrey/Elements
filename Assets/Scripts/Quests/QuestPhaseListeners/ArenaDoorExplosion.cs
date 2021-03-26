using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaDoorExplosion : QuestPhaseListener
{
    public GameObject child;

    public override void _action()
    {
        child.SetActive(true);
        child.GetComponent<ParticleSystem>().Play();
        gameObject.SetActive(false);
    }
}
