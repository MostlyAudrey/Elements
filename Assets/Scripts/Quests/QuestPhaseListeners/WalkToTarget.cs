using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

// The purpose of this class is for any NPC which is currently engaging the player in a quest to walk the path to the specified end waypoint
public class WalkToTarget : QuestPhaseListener
{
    private bool atTarget = false;
    public bool enabled = true;
    public bool isNPC = false;

    public GameObject path;
    public GameObject[] waypoints;

    private int currWaypoint = -1;
    private NavMeshAgent navMeshAgent;
    private VelocityReporter velocityReporter;
    private Vector3 prevVelocity;

    public enum Action
    {
        Idle,
        Walking
    }

    public Action action = Action.Idle;
    private Animator anim;

    private void setNextWaypoint()
    {
        try
        {
            if (currWaypoint == path.transform.childCount-1)
            {
                if (!isNPC) anim.SetBool("walking", false);
                else {
                    Debug.Log("Finished");
                    anim.SetFloat("vely", 0);
                    anim.SetFloat("velx", 0);
                }
                action = Action.Idle;
            }
            else
            {
                currWaypoint = (currWaypoint + 1) % waypoints.Length;
                navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
            }            
        }
        catch
        {
            //Debug.Log ( "Next Waypoint cannot be set due to array indexing issue or array is of length 0 " );
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (enabled)
        {
            path.SetActive(false);
            anim = gameObject.GetComponent<Animator>();
            navMeshAgent = GetComponent<NavMeshAgent>();
            switch (action)
            {
                case Action.Idle:
                    break;
                case Action.Walking:
                    if (!isNPC)
                    {
                        anim.SetBool("walking", true);
                    }
                    if (path)
                    {
                        waypoints = new GameObject[path.transform.childCount];
                        for (int i = 0; i < path.transform.childCount; i++)
                        {
                            waypoints[i] = path.transform.GetChild(i).gameObject;
                        }
                    }
                    if (currWaypoint == path.transform.childCount - 1)
                    {
                        if (!isNPC)
                        {
                            anim.SetBool("walking", false);
                        }
                        atTarget = true;
                        action = Action.Idle;
                        break;
                    }
                    else
                    {
                        setNextWaypoint();
                    }
                    break;
                default:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enabled)
        {
            if (path && (path.transform.childCount != waypoints.Length))
            {
                waypoints = new GameObject[path.transform.childCount];
                for (int i = 0; i < path.transform.childCount; i++)
                {
                    waypoints[i] = path.transform.GetChild(i).gameObject;
                }
            }
            if (action == Action.Walking)
            {
                if (navMeshAgent.remainingDistance < .5 && !navMeshAgent.pathPending)
                {
                    setNextWaypoint();
                }
                anim.SetFloat("vely", .7f);
                anim.SetFloat("velx", 0f);
            }
            if (action == Action.Idle)
            {
                anim.SetFloat("vely", 0f);
                anim.SetFloat("velx", 0f); 
            }
        }
    }

    // This function will be the action performed when the quest specified reaches the phase to listen for
    public override void _action()
    {
        
        action = Action.Walking;
        if (!isNPC)
        {
            anim.SetBool("walking", true);
        }
        path.SetActive(true);
    }

    public bool isAtTarget()
    {
        return atTarget;
    }
}
