using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class PortalDeactivator : QuestPhaseListener
{
    void _action()
    {
        foreach ( teleport teleporter in GetComponentsInChildren<teleport>() )
        {
            teleporter.gameObject.SetActive(false);
        }
    }
}