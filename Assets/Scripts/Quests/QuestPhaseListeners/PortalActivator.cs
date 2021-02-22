using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalActivator : QuestPhaseListener
{
    void _action()
    {
        foreach (teleport teleporter in GetComponentsInChildren<teleport>() )
        {
            teleporter.gameObject.SetActive(true);
        }
    }
}