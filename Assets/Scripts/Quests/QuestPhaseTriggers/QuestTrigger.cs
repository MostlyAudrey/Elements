using System;
using System.Collections.Generic;
using UnityEngine;

/*
    * QuestTrigger class
    * By Luka Derado
    * 
    * Built to handle the observation of conditions needed to progress in a quest.
    * */
public class QuestTrigger : MonoBehaviour
{
    public QuestName quest;             // Associated Quest.
    public  GameObject target;

    // If set to anything other than -1 the interactable will be active until the Quest is in a greater phase
    public int nextPhase = -1;               // The next phase once the trigger conditions are met.

    public GameObject buttonHint;       // A corresponding game object that 'holds a hint' related to the completion of the quest.
    public bool showButtonHint    = true;         // Will the corresponding hint be shown?
    public float buttonHintRadius = 15.0f;
    protected bool showingHint;            // Is it currently being shown?

    public virtual void enableTrigger(){}

    public virtual void disableTrigger(){ _hideButtonHint(); }

    void Update()
    {
        if ( showButtonHint )
        {
            if ( !target ) 
                target = GameObject.FindGameObjectWithTag("Player");
            if ( !showingHint && Vector3.Distance(target.transform.position, transform.position) <= buttonHintRadius)
      		    _displayButtonHint();
      	    else if ( showingHint && Vector3.Distance(target.transform.position, transform.position) > buttonHintRadius )
      		    _hideButtonHint();
        }
    }
    protected virtual void _displayButtonHint()
    {
        showingHint = true;
        if (showButtonHint) buttonHint.SetActive(true);
    }

    protected void _hideButtonHint()
    {
        showingHint = false;
        if (showButtonHint) buttonHint.SetActive(false);
    }

    // Method that handles advancing a quest.
    // Ideally, the 'trigger owner' will fire advance quest when certain conditions are met.
    public virtual void AdvanceQuest()
    {
        if (nextPhase < 0) // Less than zero will indicate a simple increment in quest phase.
            QuestManager.ProgressQuest(quest);
        else
            QuestManager.ProgressQuestToPhase(quest, nextPhase);
    }
}
