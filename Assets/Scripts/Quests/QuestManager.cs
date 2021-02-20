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

    private void _instantiate_quests()
    {
        // All quests have to be added to QuestName and then added here
        QuestManager.quests.Add( 
            QuestName.PerformDiagnostics, 
            new Quest( 
                QuestName.PerformDiagnostics,
                new List<QuestName>{},
                4 ) 
        );

        QuestManager.quests.Add( 
            QuestName.GettingALayOfTheLand, 
            new Quest( 
                QuestName.GettingALayOfTheLand, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                3 )
        );

        QuestManager.quests.Add( 
            QuestName.CarlTheJarl,
            new Quest(
                QuestName.CarlTheJarl,
                new List<QuestName>{ QuestName.PerformDiagnostics },
                6 ) 
        );

        QuestManager.quests.Add( 
            QuestName.ForGlory, 
            new Quest( 
                QuestName.ForGlory, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                6 )
        );

        QuestManager.quests.Add( 
            QuestName.AnIdiotsLuggage, 
            new Quest( 
                QuestName.AnIdiotsLuggage, 
                new List<QuestName>{ QuestName.PerformDiagnostics },
                3 ) 
        );

    }

}
