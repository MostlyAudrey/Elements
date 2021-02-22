using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalDeactivator : QuestPhaseListener
{
    void _action()
    {
        foreach ( Teleport teleporter in GetComponentsInChildren<Teleport>() )
        {
            teleporter.gameObject.SetActive(false);
        }
    }
}