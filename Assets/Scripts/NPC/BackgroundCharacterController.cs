using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using UnityEngine;
using UnityEngine.AI;
using System.Runtime.Versioning;
using System.ComponentModel.Design;

[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(Collider))]

public class BackgroundCharacterController : MonoBehaviour
{
    public GameObject path;
    public GameObject[] waypoints;

    private int currWaypoint = -1;
    private NavMeshAgent navMeshAgent;
    private VelocityReporter velocityReporter;
    private Vector3 prevVelocity;

    public enum Action
    {
        Idle,
        Fishing,
        SwingingTool,
        NervousLookingAround,
        Laughing,
        Walking,
        Sitting,
        Typing,
        Watering,
        Cheering,
        Clapping,
        SittingAndCheering,
        SittingAndClapping
    }

    public Action action = Action.Idle;
    private Animator anim;

    private void setNextWaypoint()
    {
        try
        {
            currWaypoint = (currWaypoint + 1) % waypoints.Length;
            navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
        }
        catch
        {
            //Debug.Log ( "Next Waypoint cannot be set due to array indexing issue or array is of length 0 " );
        }
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        switch (action)
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
            case Action.Sitting:
                anim.SetBool("sitting", true);
                break;
            case Action.Typing:
            	  anim.SetBool("typing", true);
                break;
            case Action.Watering:
                anim.SetBool("watering", true);
                break;
            case Action.Cheering:
                anim.SetBool("cheering", true);
                break;
            case Action.Clapping:
                anim.SetBool("clapping", true);
                break;
            case Action.SittingAndCheering:
                anim.SetBool("sittingandcheering", true);
                break;
            case Action.SittingAndClapping:
                anim.SetBool("sittingandclapping", true);
                break;
            case Action.Walking:
                anim.SetBool("walking", true);
                if (path)
                {
                    waypoints = new GameObject[path.transform.childCount];
                    for (int i = 0; i < path.transform.childCount; i++)
                    {
                        waypoints[i] = path.transform.GetChild(i).gameObject;
                    }
                }
                setNextWaypoint();
                break;
            default:
                break;
        }

    }

    void Update()
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
            anim.SetFloat("vely", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
            anim.SetFloat("velx", (prevVelocity.x - navMeshAgent.velocity.x) / navMeshAgent.speed);
        }
    }
}
