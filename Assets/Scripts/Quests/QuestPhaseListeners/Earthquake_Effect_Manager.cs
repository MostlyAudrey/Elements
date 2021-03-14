using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Earthquake_Effect_Manager : QuestPhaseListener
{
    private bool isActive = false;
    public float timeBetweenShakes;
    public CallbackTimer callbackTimer;
    private Camera_Shake shake;

    public override void _action()
    {
        isActive = true;
    }

    void Start()
    {
        callbackTimer = gameObject.GetComponent<CallbackTimer>();
    }

    void Update()
    {
        if (isActive)
        {
            if (callbackTimer.active == false)
            {
                callbackTimer.setTimer(timeBetweenShakes, this._shake);
            }
        }
    }

    void _shake()
    {
        shake = gameObject.GetComponent<Camera_Shake>();
        shake.CamShake();
    }
}
