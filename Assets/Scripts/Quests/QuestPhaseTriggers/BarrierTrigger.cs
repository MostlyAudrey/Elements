using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierTrigger : QuestTrigger
{
    bool playerInArea = false;
    public GameObject barrier;
    public GameObject spectator;
    public GameObject opponent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (barrier != null && opponent != null && playerInArea)
        {
            if (spectator != null) // and if the spectator is at its target
            {
            }
            else
            {
            }
            barrier.SetActive(true);
            opponent.SetActive(true);
            AdvanceQuest();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerInArea = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            playerInArea = false;
    }
}
