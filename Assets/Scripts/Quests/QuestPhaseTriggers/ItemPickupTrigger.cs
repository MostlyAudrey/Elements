using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupTrigger : QuestTrigger
{
    public void inPosition()
    {
        Debug.Log("Item pickup in position");
        AdvanceQuest();
        if ( !target ) 
            target = GameObject.FindGameObjectWithTag("Player");
        target.GetComponent<RootMotionControlScript>().destroy_picked_up_item();
        disableTrigger();
    }
}