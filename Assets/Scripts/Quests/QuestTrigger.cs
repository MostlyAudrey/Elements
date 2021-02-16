using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
    * QuestTrigger class
    * By Luka Derado
    * 
    * Built to handle the observation of conditions needed to progress in a quest.
    * */
public abstract class QuestTrigger : MonoBehaviour
{
    public QuestName quest;             // Associated Quest.

    // If set to anything other than -1 the interactable will be active until the Quest is in a greater phase
    public int nextPhase = -1;               // The next phase once the trigger conditions are met.
    public MonoBehaviour triggerOwner;  // Associated object for the trigger.

    public GameObject buttonHint;       // A corresponding game object that 'holds a hint' related to the completion of the quest.
    public bool showButtonHint;         // Will the corresponding hint be shown?
    public bool showingHint;            // Is it currently being shown?
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
        Debug.Log("AdvanceQuest");
        if (quest != QuestName.None)
            if (nextPhase < 0) // Less than zero will indicate a simple increment in quest phase.
                QuestManager.ProgressQuest(quest);
            else
                QuestManager.ProgressQuestToPhase(quest, nextPhase);
    }
}
