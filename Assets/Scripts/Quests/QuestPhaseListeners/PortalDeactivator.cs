using System;
using System.Collections.Generic;
using UnityEngine;

public class PortalDeactivator : QuestPhaseListener
{
    public override void _action()
    {
        gameObject.GetComponent<portalSetup>().enabled = false;
        foreach ( Transform child in transform.GetComponentsInChildren<Transform>(true) )
        {
            if (child.name == "teleporter" || child.name == "screen")
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}