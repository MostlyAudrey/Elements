using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestName name;

    public int totalPhases;

    public int currentPhase;

    public Quest(QuestName name, int totalPhases, int currentPhase = 0)
    {  
        this.name = name;
        this.totalPhases = totalPhases;
        this.currentPhase = currentPhase;
    }
}