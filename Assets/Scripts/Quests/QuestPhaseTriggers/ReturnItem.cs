using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Utility;

public class ReturnItem : Interactable
{
    public GameObject itemToBeReturned;
    public bool destroyItem = true;

    public float pickupRadius = 10f;

    void Update()
    {
        if ( Helper.WithinRadius( transform.position, itemToBeReturned.transform.position, pickupRadius ) )
        {
            Debug.Log("Delivered item");
            if (destroyItem)
            {
                itemToBeReturned.SetActive(false);
            }
            disableTrigger();
            AdvanceQuest();
            this.enabled = false;
        }
    }

    
}