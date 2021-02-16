using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    // The elements at each index in each of these lists should coorespond to each other  
    // example dialogs[0] is only active if quests[0] is at questPhase[0]
	public Interactable[] interactables;
	public List<QuestName> quests;
	public int[] startQuestPhase;
    public int[] endQuestPhase;

    // When activeOption is negative, nothing is active
    private int activeOption = -1;
    private bool[] questActive;

    void Start()
    {
        foreach ( Interactable option in interactables ) option.enabled = false;
        questActive = new bool[startQuestPhase.Length];
        EventManager.instance.onQuestProgressed += _check_interactable_options;
        for ( int i =0; i < startQuestPhase.Length; i++ )
        {
            if ( startQuestPhase[i] == 0 )
                questActive[i] = true;
            // if ( interactables[i].progressesQuestToPhase != -1 )
            //     interactables[i].enabled = true;
        }
    }

    void Update()
    {
        // If nothing is not active see if it should be
        if ( activeOption < 0 )
        {
            int heighestRank = 99999;
            for ( int i = 0; i < questActive.Length; i++ )
            {
                if ( questActive[i] && interactables[i].rank < heighestRank )
                {
                    heighestRank = interactables[i].rank;
                    activeOption = i;
                }
            }

        }
        else if (!interactables[activeOption].enabled)
            interactables[activeOption].enabled = true;

    }

    private void _check_interactable_options(QuestName quest, int phase)
    {
        if ( activeOption >= 0 && quest == quests[activeOption] )
        {
            questActive[activeOption] = false;
            interactables[activeOption].enabled = false;
            activeOption = -1;
        }

        for ( int i = 0; i < interactables.Length; i++)
        {
            if ( quests[i] == quest && ( startQuestPhase[i] <= phase && endQuestPhase[i] >= phase ) )
                questActive[i] = true;
        }
    }

}