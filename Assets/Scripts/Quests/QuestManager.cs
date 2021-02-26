using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{  

    static Dictionary<QuestName, Quest> quests;

    void Start()
    {
        QuestManager.quests = new Dictionary<QuestName, Quest>();
        _instantiate_quests();
    }

    public static void ProgressQuest( QuestName quest )
    {
        ProgressQuestToPhase(quest, QuestManager.quests[quest].currentPhase + 1);
    }

    public static void ProgressQuestToPhase( QuestName quest, int phase )
    {
        QuestManager.quests[quest].currentPhase = phase;
        EventManager.instance.QuestProgressed( quest, quests[quest].currentPhase );
        if ( QuestManager.quests[quest].currentPhase >= QuestManager.quests[quest].totalPhases )
            CompletedQuest(quest);
    }

    public static void CompletedQuest( QuestName completedQuest )
    {
        foreach(KeyValuePair<QuestName, Quest> entry in quests)
        {
            QuestName currName = entry.Key;
            Quest curr = entry.Value;
            if ( curr.currentPhase == -1 && curr.prerequisites.Contains(completedQuest) )
            {
                curr.prerequisites.Remove(completedQuest);

                if ( curr.prerequisites.Count == 0 )
                    ProgressQuest(currName);
            }
        }
    }

    public static int CheckQuestPhase( QuestName quest )
    {
        return QuestManager.quests[quest].currentPhase;
    }

    public static Dictionary<QuestName, Quest> GetQuests()
    {
        return quests;
    }

    public static List<Quest> GetAvailableQuests()
    {
        List<Quest> availableQuests = new List<Quest>();
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            Quest curr = entry.Value;
            if (curr.currentPhase >= 0 && curr.currentPhase < curr.totalPhases)
            {
                availableQuests.Add(curr);
            }
        }

        return availableQuests;
    }

    private void _instantiate_quests()
    {
        // All quests have to be added to QuestName and then added here
        QuestManager.quests.Add( 
            QuestName.PerformDiagnostics, 
            new Quest( 
                QuestName.PerformDiagnostics,
                new List<QuestName>{},
                new List<( string hint, string image )> {
                    ( "Automatic", null ),
                    ( "Turn on the first portal by hitting the button", "Images/test" ),
                    ( "Go through the newly active portal and collect your weapon", "Images/test" ),
                    ( "Return through the portal and destroy the box containing your tools", "Images/test" ),
                    ( "Use your tools to fix the generator" , "Images/test" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.GettingALayOfTheLand, 
            new Quest( 
                QuestName.GettingALayOfTheLand, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Automatic", null ),
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test" ),
                    ( "Talk to the Shogun of Bakufu", "Images/test" ),
                    ( "Talk to the Chief of Fimbultoft", "Images/test" ),
                })
        );

        QuestManager.quests.Add( 
            QuestName.CarlTheJarl,
            new Quest(
                QuestName.CarlTheJarl,
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Chief of Fimbultoft", "Images/test.jpg" )
                }) 
        );

        QuestManager.quests.Add( 
            QuestName.ForGlory, 
            new Quest( 
                QuestName.ForGlory, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Shogun of Bakufu", "Images/test.jpg" )
                })
        );

        QuestManager.quests.Add( 
            QuestName.AnIdiotsLuggage, 
            new Quest( 
                QuestName.AnIdiotsLuggage, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                new List<( string hint, string image )> {
                    ( "Talk to the Bank Manager at Morgan's Panley", "Images/test.jpg" )
                }) 
        );
    }

    public static void LoadQuestPhases(PlayerData data)
    {
        int i = 0;
        foreach (KeyValuePair<QuestName, Quest> entry in quests)
        {
            QuestName currName = entry.Key;
            ProgressQuestToPhase(currName, data.GetQuestPhase(i)); //Makes sure prereqs get updated too
            ++i;
        }
    }
}
