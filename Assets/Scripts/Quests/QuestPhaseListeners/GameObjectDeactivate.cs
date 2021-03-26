using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectDeactivate : QuestPhaseListener
{
    public override void _action()
    {
        gameObject.SetActive(false);
    }
}
