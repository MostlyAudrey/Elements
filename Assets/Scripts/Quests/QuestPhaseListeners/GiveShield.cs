using System;
using System.Collections.Generic;
using UnityEngine;

public class GiveShield : QuestPhaseListener
{
    private RootMotionControlScript player_script;
    public override void _action()
    {
        player_script = gameObject.GetComponent<RootMotionControlScript>();
        if (player_script)
        {
            player_script.hasShield = true;
            player_script.sheathShield();
        }
        else Debug.Log("Could not find RootMotionControlScript");
    }
}