using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDeactivator : QuestPhaseListener
{
    public MonoBehaviour script;

    public override void _action()
    {
        script.enabled = false;
    }
}
