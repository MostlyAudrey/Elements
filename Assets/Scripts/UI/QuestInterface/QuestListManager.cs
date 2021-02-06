using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestListManager : MonoBehaviour
{
    public GameObject questListItemPrefab;

    // display items for in-progress quests, sorted in same order as in QuestName.cs
    private List<QuestListItem> questListItems;

    public float time = 3;                                                              

    void Start()
    {
        questListItems = new List<QuestListItem>();
    }

    void Update()
    {
    }

    public void RefreshOngoingQuests()
    {
        // Remove completed quests
        for (int i = 0; i < questListItems.Count; ++i)
        {
            Quest quest = questListItems[i].GetQuest();
            if (quest.currentPhase >= quest.totalPhases)
            {
                RemoveQuestListItem(i);
            }
        }

        // Refresh quest progress in display list items for ongoing quests
        foreach (QuestListItem item in questListItems)
        {
            item.RefreshQuestProgress();
        }

        // Add new quests
        foreach (KeyValuePair<QuestName, Quest> questKeyValPair in QuestManager.GetQuests())
        {
            Quest quest = questKeyValPair.Value;
            if (quest.currentPhase > 0 && quest.currentPhase < quest.totalPhases)
            {
                if (questListItems.Count == 0)
                {
                    AddQuestListItem(0, quest);
                }
                else
                {
                    // Check if quest is already in the list using a binary search
                    // If the quest is not in the list, insert it in order
                    int binarySearchRes = BinarySearchQuestList(0, questListItems.Count - 1, quest);
                    if (binarySearchRes < 0)
                    {
                        AddQuestListItem(-binarySearchRes, quest);
                    }
                }
            }
        }
    }

    private void AddQuestListItem(int index, Quest quest)
    {
        if (questListItemPrefab != null)
        {
            GameObject itemGameObject = Instantiate(questListItemPrefab, transform);
            if (itemGameObject != null)
            {
                // Make sure it displays in right order
                itemGameObject.transform.SetSiblingIndex(index);

                QuestListItem questListItem = itemGameObject.GetComponent<QuestListItem>();
                questListItems.Insert(index, questListItem);
                questListItem.Init(quest);
            }
        }
    }

    private void RemoveQuestListItem(int index)
    {
        Destroy(questListItems[index].gameObject);
        questListItems.RemoveAt(index);
    }

    // Helper method to perform an efficient binary search to see
    // if a given quest is already associated with a quest list item.
    // Returns 0 if there are 0 quest list items, otherwise
    // the negative complement of the higher quest list item if
    // the quest was not found.
    private int BinarySearchQuestList(int startIndex, int endIndex, Quest quest)
    {
        if (startIndex > endIndex)
        {
            // index of the list item with the quest directly higher than the given quest
            return -startIndex;
        }
        else
        {
            int mid = (startIndex + endIndex) / 2;
            if (quest == questListItems[mid].GetQuest())
            {
                return mid;
            }
            else if ((int)quest.name > (int)questListItems[mid].GetQuest().name)
            {
                return BinarySearchQuestList(mid + 1, endIndex, quest);
            }
            else
            {
                return BinarySearchQuestList(startIndex, mid - 1, quest);
            }
        }
    }
}
