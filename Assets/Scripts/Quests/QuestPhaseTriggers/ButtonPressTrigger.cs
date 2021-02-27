using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/*
 *  Trigger is remarkably similar to the ReturnItem extension of interactable.
 */
class ButtonPressTrigger : QuestTrigger
{
    public GameObject moveableComponent;

    public override void enableTrigger()
    {
        gameObject.tag = "Button";
    }

    public override void disableTrigger()
    {
        gameObject.tag = "Untagged";
        _hideButtonHint();
    }

    public void pushButton()
    {
        AdvanceQuest();
    }
}
