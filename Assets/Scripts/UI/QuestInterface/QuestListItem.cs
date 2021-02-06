using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestListItem : MonoBehaviour
{
    public GameObject image;
    public GameObject title;
    public GameObject progressBar;

    private Quest quest;

    void Start()
    {   
    }

    void Update()
    {   
    }

    // Call after instantiation
    public void Init(Quest quest)
    {
        this.quest = quest;
        string questName = quest.name.ToString();

        // Set quest image
        string picPath = "QuestInterface/Pictures/" + questName;
        Texture2D picData = Resources.Load<Texture2D>(picPath); // for some reason sprites are not loading
        if (picData == null)
        {
            Debug.Log(picPath);
        }
        else
        {
            Sprite picData2 = Sprite.Create(picData, new Rect(0f, 0f, picData.width, picData.height), new Vector2(0.5f, 0.5f));
            if (image != null)
            {
                image.GetComponent<Image>().overrideSprite = picData2;
            }
        }

        // Set quest description
        //TextAsset descData = Resources.Load<TextAsset>("QuestInterface/Descriptions/" + questName);
        //if (descData != null)
        //{
            //string desc = descData.ToString();
            //Text txtComp = description.GetComponent<Text>();
            //if (txtComp != null)
            //{
                //txtComp.text = desc;
            //}
        //}

        // Set quest title
        if (title != null)
        {
            title.GetComponent<Text>().text = questName;
        }

        RefreshQuestProgress();
    }

    // Call every time pause menu opens
    public void RefreshQuestProgress()
    {
        float percentProgress = ((float)quest.currentPhase) / quest.totalPhases;
        ProgressBar progressBarComp = progressBar.GetComponent<ProgressBar>();
        if (progressBarComp != null)
        {
            progressBarComp.SetPercentFill(percentProgress);
        }
    }

    public Quest GetQuest()
    {
        return quest;
    }
}
