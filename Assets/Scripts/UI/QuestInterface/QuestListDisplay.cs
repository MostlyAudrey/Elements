using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListDisplay : MonoBehaviour
{
    public GameObject availableQuestsListRoot;
    public GameObject inprogressQuestsListRoot;
    public GameObject listItemPrefab;

    private List<QuestListItem> listItems;                                                        

    void Start()
    {
        listItems = new List<QuestListItem>();
    }

    public void OpenDisplay()
    {
        List<Quest> availableQuests = QuestManager.GetAvailableQuests();
        for (int i = 0; i < availableQuests.Count; ++i)
        {
            // Filter out in-progress quests
            if (availableQuests[i].currentPhase > 0)
            {
                AddQuestListItem(availableQuests[i], inprogressQuestsListRoot.transform);
            }
            else
            {
                AddQuestListItem(availableQuests[i], availableQuestsListRoot.transform);
            }
            Debug.Log(availableQuests[i].name);
        }
    }

    public void CloseDisplay()
    {
        for (int i = listItems.Count - 1; i >= 0; --i)
        {
            RemoveQuestListItemAt(i);
        }
    }

    // public void RefreshOngoingQuests()
    // {
    //     // Remove completed quests
    //     for (int i = 0; i < questListItems.Count; ++i)
    //     {
    //         Quest quest = questListItems[i].GetQuest();
    //         if (quest.currentPhase >= quest.totalPhases)
    //         {
    //             RemoveQuestListItem(i);
    //         }
    //     }

    //     // Refresh quest progress in display list items for ongoing quests
    //     foreach (QuestListItem item in questListItems)
    //     {
    //         item.RefreshQuestProgress();
    //     }

    //     // Add new quests
    //     foreach (KeyValuePair<QuestName, Quest> questKeyValPair in QuestManager.GetQuests())
    //     {
    //         Quest quest = questKeyValPair.Value;
    //         if (quest.currentPhase > 0 && quest.currentPhase < quest.totalPhases)
    //         {
    //             if (questListItems.Count == 0)
    //             {
    //                 AddQuestListItem(0, quest);
    //             }
    //             else
    //             {
    //                 // Check if quest is already in the list using a binary search
    //                 // If the quest is not in the list, insert it in order
    //                 int binarySearchRes = BinarySearchQuestList(0, questListItems.Count - 1, quest);
    //                 if (binarySearchRes < 0)
    //                 {
    //                     AddQuestListItem(-binarySearchRes, quest);
    //                 }
    //             }
    //         }
    //     }
    // }

    private void AddQuestListItem(Quest quest, Transform root)
    {
        if (listItemPrefab != null)
        {
            GameObject listItem = Instantiate(listItemPrefab, root);
            if (listItem != null)
            {
                // // Make sure it displays in right order
                // itemGameObject.transform.SetSiblingIndex(index);
                QuestListItem comp = listItem.GetComponent<QuestListItem>();
                comp.Init(quest);
                listItems.Add(comp);
            }
            else
            {
                Debug.Log("QuestListManager: list item instantiation unsuccessful!");
            }
        }
        else
        {
            Debug.Log("QuestListManager: listItemPrefab is null!");
        }
    }

    private void RemoveQuestListItemAt(int index)
    {
        if (listItems[index] != null)
        {
            // Destroys the list item object at the end of the frame
            Destroy(listItems[index].gameObject);
        }
        listItems.RemoveAt(index);
    }

    // // Helper method to perform an efficient binary search to see
    // // if a given quest is already associated with a quest list item.
    // // Returns 0 if there are 0 quest list items, otherwise
    // // the negative complement of the higher quest list item if
    // // the quest was not found.
    // private int BinarySearchQuestList(int startIndex, int endIndex, Quest quest)
    // {
    //     if (startIndex > endIndex)
    //     {
    //         // index of the list item with the quest directly higher than the given quest
    //         return -startIndex;
    //     }
    //     else
    //     {
    //         int mid = (startIndex + endIndex) / 2;
    //         if (quest == questListItems[mid].GetQuest())
    //         {
    //             return mid;
    //         }
    //         else if ((int)quest.name > (int)questListItems[mid].GetQuest().name)
    //         {
    //             return BinarySearchQuestList(mid + 1, endIndex, quest);
    //         }
    //         else
    //         {
    //             return BinarySearchQuestList(startIndex, mid - 1, quest);
    //         }
    //     }
    // }
}
