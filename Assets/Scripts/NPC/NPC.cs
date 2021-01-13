using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{

    // The elements at each index in each of these lists should coorespond to each other  
    // example dialogs[0] is only active if quests[0] is at questPhase[0]
	public DialogOption[] dialogs;
	public List<QuestName> quests;
	public int[] questPhase;
    

    // When activeOption is negative, nothing is active
    private int activeOption = -1;
    private bool[] questActive;

    void Start()
    {
        foreach ( DialogOption opt in dialogs ) opt.enabled = false;
        questActive = new bool[questPhase.Length];
        EventManager.instance.onQuestProgressed += _check_dialog_options;
        for ( int i =0; i < questPhase.Length; i++ )
            if ( questPhase[i] == 0 )
                questActive[0] = true;
    }

    void Update()
    {
        // If nothing is not active see if it should be
        if ( activeOption < 0 )
        {
            int heighestRank = 99999;
            for ( int i = 0; i < questActive.Length; i++ )
            {
                if ( questActive[i] && dialogs[i].rank < heighestRank )
                {
                    heighestRank = dialogs[i].rank;
                    activeOption = i;
                }
            }

        }
        else if (!dialogs[activeOption].enabled)
            dialogs[activeOption].enabled = true;

    }

    private void _check_dialog_options(QuestName quest, int phase)
    {
        if ( activeOption >= 0 && quest == quests[activeOption] )
        {
            questActive[activeOption] = false;
            dialogs[activeOption].enabled = false;
            activeOption = -1;
        }

        for ( int i = 0; i < dialogs.Length; i++)
        {
            if ( quests[i] == quest && questPhase[i] == phase )
                questActive[i] = true;
        }
    }

}