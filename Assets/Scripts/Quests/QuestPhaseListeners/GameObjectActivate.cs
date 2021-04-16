using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActivate : QuestPhaseListener
{
    public GameObject target;
    public override void _action()
    {
        target.SetActive(true);
    }
}
