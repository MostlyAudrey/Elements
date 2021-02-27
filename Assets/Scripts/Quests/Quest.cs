using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest
{
    public QuestName name;

    public List<QuestName> prerequisites;

    public int numPrereqsRequired;

    public int prereqsCompleted = 0;

    public List<( string hint, string image )> phaseHints;

    public int totalPhases;

    public int currentPhase;

    public Quest(QuestName name, List<QuestName> prerequisites, List<( string hint, string image )> phaseHints, int numPrereqsRequired = 999)
    {  
        this.name = name;
        this.prerequisites = prerequisites;
        this.phaseHints = phaseHints;
        this.totalPhases = phaseHints.Count;
        this.currentPhase = prerequisites.Count > 0 ? -1: 0;
        this.numPrereqsRequired = numPrereqsRequired;

        if (this.currentPhase == 0) EventManager.instance.QuestProgressed( name, 0 );
        Debug.Log(name + ": " + currentPhase);
    }

    public ( string hint, string image ) getPhaseHint()
    {
        return phaseHints[currentPhase];
    }
}