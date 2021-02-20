using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestName name;

    public List<QuestName> prerequisites;

    public int totalPhases;

    public int currentPhase;

    public Quest(QuestName name, List<QuestName> prerequisites, int totalPhases)
    {  
        this.name = name;
        this.prerequisites = prerequisites;
        this.totalPhases = totalPhases;
        this.currentPhase = prerequisites.Count > 0 ? -1: 0;
    }
}