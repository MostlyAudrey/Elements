using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectActivate : QuestPhaseListener
{
    public override void _action()
    {
        gameObject.SetActive(true);
    }
}
