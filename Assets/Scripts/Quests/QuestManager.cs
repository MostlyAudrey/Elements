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
        QuestManager.quests[quest].currentPhase++;
        EventManager.instance.QuestProgressed( quest, quests[quest].currentPhase );
    }

    public static void ProgressQuestToPhase( QuestName quest, int phase )
    {
        QuestManager.quests[quest].currentPhase = phase;
        EventManager.instance.QuestProgressed( quest, quests[quest].currentPhase );
    }

    public static int CheckQuestPhase( QuestName quest )
    {
        return QuestManager.quests[quest].currentPhase;
    }

    private void _instantiate_quests()
    {
        // All quests have to be added to QuestName and then added here
        QuestManager.quests.Add( QuestName.IntroductionQuest, new Quest( QuestName.IntroductionQuest, 4 ) );
    }

}
