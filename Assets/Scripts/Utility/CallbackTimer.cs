using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CallbackTimer : MonoBehaviour
{
    public float timeRemaining;
    public bool active = false;

    public delegate void onTimer();
    public onTimer Todo;
    void Update()
    {
        if (active)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            } else
            {
                active = false;
                if (Todo != null)
                {
                    Todo();
                }
            }
        }
        
    }

    public void setTimer(float time, onTimer callback)
    {
        timeRemaining = time;
        Todo = callback;
        active = true;
    }
}
