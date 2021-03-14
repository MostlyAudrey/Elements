using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;

public class ReturnItem : Interactable
{
    public GameObject itemToBeReturned;

    public float pickupRadius = 10f;

    void Update()
    {
        if ( Helper.WithinRadius( transform.position, itemToBeReturned.transform.position, pickupRadius ) )
        {
            Debug.Log("Delivered item");
            itemToBeReturned.SetActive( false );
            disableTrigger();
            AdvanceQuest();
        }
    }

    
}