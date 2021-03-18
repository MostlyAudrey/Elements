using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListWidget : MonoBehaviour
{
    public GameObject questListRoot;
    public GameObject listItemPrefab;

    private List<QuestListItem> listItems;                                                        

    void OnEnable()
    {
        OpenDisplay();
    }

    void OnDisable()
    {
        CloseDisplay();
    }

    public void OpenDisplay()
    {
        List<Quest> availableQuests = QuestManager.GetAvailableQuests();
        
        List<Quest> inProgress = new List<Quest>();
        List<Quest> ready2Start = new List<Quest>();

        foreach (Quest quest in availableQuests)
        {
            //Filter based on progress
            if (quest.currentPhase > 0)
            {
                inProgress.Add(quest);
            }
            else
            {
                ready2Start.Add(quest);
            }
        }

        //Add in-progress quests before quests that have yet to be started
        foreach (Quest quest in inProgress)
        {
            AddQuestListItem(quest);
        }

        foreach (Quest quest in ready2Start)
        {
            AddQuestListItem(quest);
        }
    }

    public void CloseDisplay()
    {
        for (int i = listItems.Count - 1; i >= 0; --i)
        {
            RemoveQuestListItemAt(i);
        }
        listItems = null;
    }

    private void AddQuestListItem(Quest quest)
    {
        if (listItemPrefab != null)
        {
            QuestListItem questListItem = Instantiate(listItemPrefab, questListRoot.transform).GetComponent<QuestListItem>();
            questListItem.Init(quest);
            
            if (listItems == null)
            {
                listItems = new List<QuestListItem>();
            }
            listItems.Add(questListItem);
        }
        else
        {
            Debug.LogError("QuestListDisplay: listItemPrefab is null!");
        }
    }

    private void RemoveQuestListItemAt(int index)
    {
        if (listItems[index] != null)
        {
            //Destroy the quest list item object at the end of the frame
            Destroy(listItems[index].gameObject);
        }
        listItems.RemoveAt(index);
    }
}
