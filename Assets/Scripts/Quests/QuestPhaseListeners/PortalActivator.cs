using System;
using System.Collections.Generic;
using UnityEngine;

public class PortalActivator : QuestPhaseListener
{
    public override void _action()
    {
        foreach ( Transform child in transform.GetComponentsInChildren<Transform>(true) )
        {
            if (child.name == "teleporter" || child.name == "screen")
            {
                child.gameObject.SetActive(true);
            }
        }
        gameObject.GetComponent<portalSetup>().enabled = true;

    }
}