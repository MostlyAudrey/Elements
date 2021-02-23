using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class QuestListItem : MonoBehaviour
{
    public Image image;
    public Text title;
    public ProgressBar progressBar;

    private static Regex camelCaseDelimiter;

    // Call after instantiation
    public void Init(Quest quest)
    {
        string questName = quest.name.ToString();

        // Set quest image
        string picPath = "QuestInterface/Pictures/" + questName;
        Texture2D texData = Resources.Load<Texture2D>(picPath); // for some reason sprites are not loading
        if (texData != null)
        {
            Sprite spriteData = Sprite.Create(texData, new Rect(0f, 0f, texData.width, texData.height), new Vector2(0.5f, 0.5f));
            image.overrideSprite = spriteData;
        }
        else
        {
            Debug.Log(picPath + " did not load!");
        }

        // Set quest title
        // Put spaces in between camelCase of quest name
        if (camelCaseDelimiter == null)
        {
            camelCaseDelimiter = new Regex("(\\B[A-Z])");
        }
        string questTitle = camelCaseDelimiter.Replace(questName, " $1");
        title.text = questTitle;

        // Set quest progress
        float percentProgress = ((float)quest.currentPhase) / quest.totalPhases;
        progressBar.SetPercentFill(percentProgress);

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
    }

    // // Call every time pause menu opens
    // public void RefreshQuestProgress()
    // {
    //     float percentProgress = ((float)quest.currentPhase) / quest.totalPhases;
    //     ProgressBar progressBarComp = progressBar.GetComponent<ProgressBar>();
    //     if (progressBarComp != null)
    //     {
    //         progressBarComp.SetPercentFill(percentProgress);
    //     }
    // }

    // public Quest GetQuest()
    // {
    //     return quest;
    // }
}
