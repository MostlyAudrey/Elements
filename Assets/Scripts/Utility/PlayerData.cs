using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * Class representing a save file. Any primitive instance variable
 * in this class will be serialized.
 *
 * Made by Aneet Nadella and Akash C.
 */
[System.Serializable]
public class PlayerData
{
    private long dateTime;
    private float[] location;
    private float[] rotation;
    private float[] scale;
    private bool sword = false;
    private bool shield = false;
    private int[] questPhases;

    public PlayerData()
    {
        dateTime = System.DateTime.Now.ToBinary();

        RootMotionControlScript rootMotionControl =
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<RootMotionControlScript>(true);
        if (rootMotionControl == null)
        {
            Debug.LogError("PlayerData could not find RootMotionControlScript object");
            return;
        }

        // Save each location coordinate as a float element
        Vector3 charPosition = rootMotionControl.transform.position;
        location = new float[3];
        location[0] = charPosition.x;
        location[1] = charPosition.y;
        location[2] = charPosition.z;

        Quaternion charRotation = rootMotionControl.transform.rotation;
        rotation = new float[4];
        rotation[0] = charRotation.x;
        rotation[1] = charRotation.y;
        rotation[2] = charRotation.z;
        rotation[3] = charRotation.w;

        Vector3 charScale = rootMotionControl.transform.localScale;
        scale = new float[3];
        scale[0] = charScale.x;
        scale[1] = charScale.y;
        scale[2] = charScale.z;

        sword =  rootMotionControl.hasSword;
        shield = rootMotionControl.hasShield;

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

    /**
     * Call after loading scene to load game with saved player data.
     */
    public void LoadGame()
    {
        RootMotionControlScript rootMotionControl =
            GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<RootMotionControlScript>(true);
        if (rootMotionControl == null)
        {
            Debug.LogError("PlayerData could not find RootMotionControlScript object");
            return;
        }

        // Deserialize location, rotation, and scale
        Vector3 newLocation = new Vector3(location[0], location[1], location[2]);
        Quaternion newRotation = new Quaternion(rotation[0], rotation[1], rotation[2], rotation[3]);
        Vector3 newScale = new Vector3(scale[0], scale[1], scale[2]);

        // Update root motion controller
        rootMotionControl.transform.SetPositionAndRotation(newLocation, newRotation);
        rootMotionControl.transform.localScale = newScale;
        rootMotionControl.hasSword = sword;
        rootMotionControl.hasShield = shield;

        // Update quest manager
        int i = 0;
        foreach (KeyValuePair<QuestName, Quest> entry in QuestManager.GetQuests())
        {
            QuestName questName = entry.Key;
            Quest quest = entry.Value;

            //May be called before Start function, so progress quests to 0 if necessary
            if (quest.currentPhase == 0)
            {
                EventManager.instance.QuestProgressed(quest.name, 0);
            }

            //Activate all QuestPhaseListeners (Iterate through each quest phase)
            for (int questPhase = quest.currentPhase; questPhase < GetQuestPhase(i); ++questPhase)
            {
                QuestManager.ProgressQuest(questName);
            }

            ++i;
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

    public System.DateTime GetDateTime()
    {
        return System.DateTime.FromBinary(dateTime);
    }
}
