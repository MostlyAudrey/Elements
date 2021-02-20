using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class KillCountTrigger : QuestTrigger
{
    public int ID;                  // semi-unique identifier for KillCountTriggers.
    public int killCount;           // current kill count
    public int goalCount;           // goal kill count
    public MobAI[] initTargets;     // initial target list. This property lets us set targets from the unity editor that exist at compile time.
    public List<MobAI> targets;     // the dynamic target list. This property lets us change the target set at run time.
    
    public KillCountTrigger(int count)
    {
        goalCount = count;

        targets.AddRange(initTargets);
    }
    public void Start()
    {
        EventManager.instance.onDeath += HandleDeath;
    }

    /* Delegate function that Mobspawns need to call
    *  in order to increase the kill count.
    */
    public void HandleDeath(MobAI victim)
    {
        if (targets.Contains(victim))
        {
            killCount++;
            targets.Remove(victim);
        }
            
        if (killCount >= goalCount)
        {
            AdvanceQuest();
            // unsubscribe to the event aggregator.
        }
    }
}
