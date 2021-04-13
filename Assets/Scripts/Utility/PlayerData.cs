using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public float[] location;
    public float[] rotation;
    public float[] scale;
    public bool sword = false;
    public bool shield = false;

    public int year;
    public int month;
    public int date;
    public int hour;
    public int minute;

    public int[] questPhases;

    public PlayerData(RootMotionControlScript playerRootMotionControl)
    {
        // Save each location coordinate as a float element
        Vector3 charPosition = playerRootMotionControl.transform.position;
        location = new float[3];
        location[0] = charPosition.x;
        location[1] = charPosition.y;
        location[2] = charPosition.z;

        Quaternion charRotation = playerRootMotionControl.transform.rotation;
        rotation = new float[4];
        rotation[0] = charRotation.x;
        rotation[1] = charRotation.y;
        rotation[2] = charRotation.z;
        rotation[3] = charRotation.w;

        Vector3 charScale = playerRootMotionControl.transform.localScale;
        scale = new float[3];
        scale[0] = charScale.x;
        scale[1] = charScale.y;
        scale[2] = charScale.z;

        sword =  playerRootMotionControl.hasSword;
        shield = playerRootMotionControl.hasShield;
        Debug.Log("Sword" + sword);
        Debug.Log("Shield" + shield);

        var dt = System.DateTime.Now;
        year = dt.Year;
        month = dt.Month;
        date = dt.Day;
        hour = dt.Hour;
        minute = dt.Minute;



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

    public Quaternion GetPlayerRotation()
    {
        if (rotation.Length >= 4)
        {
            return new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
        }
        else
        {
            Debug.LogError("Tried to load rotation data that was not saved");
            return Quaternion.identity;
        }
    }

    public Vector3 GetPlayerScale()
    {
        if (scale.Length >= 3)
        {
            return new Vector3(scale[0], scale[1], scale[2]);
        }
        else
        {
            Debug.LogError("Tried to load scale data that was not saved");
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
