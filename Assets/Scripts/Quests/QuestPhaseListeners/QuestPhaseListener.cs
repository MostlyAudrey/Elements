using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestPhaseListener : MonoBehaviour
{
    public QuestName quest;             // Associated Quest.
    public int phase_to_listen_for = 0;

    void Awake()
    {
        EventManager.instance.onQuestProgressed += questPhaseUpdateListener;
    }

    // This function will listen to every quest advancement
    private void questPhaseUpdateListener(QuestName quest, int phase){
        if (this.quest == quest && this.phase_to_listen_for == phase)
            this._action();
    }

    // This function will be the action performed when the quest specified reaches the phase to listen for
    public abstract void _action();

}
