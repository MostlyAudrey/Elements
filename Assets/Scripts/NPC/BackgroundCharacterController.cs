using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(Collider))]
public class BackgroundCharacterController : MonoBehaviour
{
    public enum Action
    {
        Idle,
        Fishing,
        SwingingTool,
        NervousLookingAround,
        Laughing
    }

    public Action action = Action.Idle;
    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        switch(action)
        {
            case Action.Idle:
                break;
            case Action.Fishing:
                anim.SetBool("fishing", true);
                break;
            case Action.SwingingTool:
                anim.SetBool("swingingTool", true);
                break;
            case Action.NervousLookingAround:
                anim.SetBool("nervousLookingAround", true);
                break;
            case Action.Laughing:
                anim.SetBool("laughing", true);
                break;
            default:
                break;
        }
    }
}