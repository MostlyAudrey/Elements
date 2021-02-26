using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] location;
    public int[] questPhases;

    public PlayerData(RootMotionControlScript playerRootMotionControl)
    {
        // Save each location coordinate as a float element
        Vector3 charPosition = playerRootMotionControl.transform.position;
        location = new float[3];
        location[0] = charPosition.x;
        location[1] = charPosition.y;
        location[2] = charPosition.z;

        // Save each current quest phase as an element in questPhases
        Dictionary<QuestName, Quest> quests = QuestManager.GetQuests();
        questPhases = new int[quests.Count];

        int i = 0;
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            Quest curr = entry.Value;
            questPhases[i] = curr.currentPhase;
            ++i;
        }
    }

    public Vector3 GetPlayerLocation()
    {
        if (location.Length >= 3)
        {
            return new Vector3(location[0], location[1], location[2]);
        }
        else
        {
            Debug.LogError("Tried to load location data that was not saved");
            return Vector3.zero;
        }
    }

    public int GetQuestPhase(int index)
    {
        if (index >= 0 && index < questPhases.Length)
        {
            return questPhases[index];
        }
        else
        {
            Debug.LogError("Tried to load quest phase data that was not saved at index: " + index);
            return -1;
        }
    }
}
