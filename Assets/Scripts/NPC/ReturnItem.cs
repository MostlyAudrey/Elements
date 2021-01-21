using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnItem : Interactable
{
    public GameObject itemToBeReturned;

    public float pickupRadius = 10f;

    void Update()
    {
        if ( ( transform.position - itemToBeReturned.transform.position ).magnitude <= pickupRadius )
        {
            Debug.Log("Delivered item");
            itemToBeReturned.SetActive( false );
            AdvanceQuest();
        }
    }

    
}