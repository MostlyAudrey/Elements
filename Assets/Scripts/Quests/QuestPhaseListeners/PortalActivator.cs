using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalActivator : QuestPhaseListener
{
    void _action()
    {
        foreach ( Teleport teleporter in GetComponentsInChildren<Teleport>() )
        {
            teleporter.gameObject.SetActive(true);
        }
    }
}