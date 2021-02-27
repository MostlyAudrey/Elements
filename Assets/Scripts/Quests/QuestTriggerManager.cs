using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTriggerManager : MonoBehaviour
{
    
    // The elements at each index in each of these lists should coorespond to each other  
    // example dialogs[0] is only active if quests[0] is at questPhase[0]
	public List<QuestTrigger> triggers;
	public List<QuestName> quests;
    
    /* At which phase in the quest the corresponding interactable should be active
    *  Each index refers to a quest.
    */
    public int[] startQuestPhase;

    /* At which point in the quest the corresponding interactable should be deactivated.
    *  Each index refers to a quest.
    */
    public int[] endQuestPhase;
   
    // Which interactable is active
    // When activeOption is negative, nothing is active 
    // private int activeOption = -1;

    // Which quests are currently active, and which are not.
    private bool[] activeTriggers;

    void Awake()
    {
        foreach ( QuestTrigger trigger in triggers ) trigger.enabled = false;
        //activeTriggers = new bool[triggers.Count];
        EventManager.instance.onQuestProgressed += _check_trigger_options;
        // for ( int i =0; i < triggers.Count; i++ )
        // {
        //     if ( startQuestPhase[i] == 0 )
        //         activeInteractables[i] = true;
        // }
    }

    void Update()
    {
        // // If nothing is not active see if something should be
        // if ( activeOption < 0 )
        // {
        //     int heighestRank = 99999;
        //     // For all quests, determine...
        //     for ( int i = 0; i < activeInteractables.Length; i++ )
        //     {
        //         // if the quest is active, and the rank of the interactables is less (higher priority?) than the highest rank.
        //         if ( activeInteractables[i] && interactables[i].rank < heighestRank )
        //         {
        //             // Set the new 'highest rank' (lowest priority?) to the corresponding interactable.
        //             heighestRank = interactables[i].rank;

        //             // Set the 'active quest' to that one.
        //             activeOption = i;
        //         }
        //     }

        // }

        // // If something is active, check if the interactables are disabled.
        // else if (!interactables[activeOption].enabled)
        //     // if so, enable them.
        //     interactables[activeOption].enabled = true;

    }

    /*  This is called every time a quest advances
     *  This function checks to see if any of the triggers it manages should turn on or off because of this quest advancement
     */
    void _check_trigger_options(QuestName quest, int phase)
    {
        // // If there is an active queset, and this is that quest...
        // if ( activeOption >= 0 && quest == quests[activeOption] )
        // {
        //     activeInteractables[activeOption] = false;              // disable it.
        //     interactables[activeOption].enabled = false;    // disable the corresponding interactables.
        //     activeOption = -1;                              // set the active quest to 'none'
        // }

        // for all interactables...
        for ( int i = 0; i < triggers.Count; i++)
        {
            // if the corresponding quest type is the particular quest type , and it is within the range of where the interactable should be online...
            if (quests[i] == quest && (startQuestPhase[i] <= phase && endQuestPhase[i] >= phase))
            {
                // Set that quest to an active quest.
                triggers[i].enableTrigger();
                triggers[i].enabled = true;
            }
            else
            {
                triggers[i].disableTrigger();
                triggers[i].enabled = false;
            }
        }
    }
}