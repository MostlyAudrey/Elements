using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class MobAI : MonoBehaviour
{
	// Either path or waypoints should be populated but not both, path has priority ofver waypoints
	// A path is the parent game object which the waypoints are childed too
	// waypoints is a list of points to go to
	public GameObject path;
	public GameObject[] waypoints;
	public GameObject target;

	private int currWaypoint = -1;
	private NavMeshAgent navMeshAgent;
	private Animator animator;
	private VelocityReporter velocityReporter;
	private Vector3 prevVelocity;

	public float animationSpeed = 1f;
	public float rootMovementSpeed = 1f;
	public float rootTurnSpeed = 1f;
    public float fallSpeed = 1f;
	public bool is_hostile = false;

	public int health = 100;
	public bool isDead = false;

	public AIState aiState;

	private void setNextWaypoint(){ 
		try {
			currWaypoint = (currWaypoint + 1) % waypoints.Length;
			navMeshAgent.SetDestination(waypoints[currWaypoint].transform.position);
		} catch {
			//Debug.Log ( "Next Waypoint cannot be set due to array indexing issue or array is of length 0 " );
		}
	}

	void Start(){
		if ( path )
		{
			waypoints = new GameObject[path.transform.childCount];
			for (int i = 0; i< path.transform.childCount; i++)
            {
                waypoints[i] = path.transform.GetChild(i).gameObject;
            }
		}
		if ( !target )
			target = GameObject.FindGameObjectWithTag("Player");
		setNextWaypoint ();
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator= GetComponent<Animator>();
		aiState = AIState.Patrol;
		velocityReporter = target.GetComponent<VelocityReporter>();
	}

	void Update()
	{ 
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (is_hostile && (PlayerInRadius() || PlayerInSight())) {
			Debug.Log("Engaging!");
			animator.SetBool("holding sword", true);
			if (aiState == AIState.Patrol) {
				aiState = AIState.InterceptTarget;
			}
		}

		if ( path && ( path.transform.childCount != waypoints.Length ) )
		{
			waypoints = new GameObject[path.transform.childCount];
			for (int i = 0; i< path.transform.childCount; i++)
            {
                waypoints[i] = path.transform.GetChild(i).gameObject;
            }
		}

		if (aiState == AIState.Patrol) {
			if (navMeshAgent.remainingDistance < .5 && !navMeshAgent.pathPending) {
				setNextWaypoint ();
			}
			animator.SetFloat ("vely", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
			animator.SetFloat ("velx", ( prevVelocity.x - navMeshAgent.velocity.x ) / navMeshAgent.speed);
		} else if (aiState == AIState.InterceptTarget) {
			try {
				navMeshAgent.stoppingDistance = 3.5f;	// stops near the player
				navMeshAgent.SetDestination(target.transform.position + velocityReporter.velocity);

				float distance = Vector3.Distance(transform.position, player.transform.position);
				// if distance between the mob and the player is _____, play the attack animation and reduce the player's health by ____. 
				// (may have to adjust this later, so that the player can counter or dodge or something)
				// also add logic for determining the distance at which the mob attacks (if it is ranged or close)
				if (distance <= 4f)
				{
					attack();
				}

				// if player moves too far away, put sword away if it is out, and go back to patrolling
				if (distance >= 25f && animator.GetBool("holding sword"))
                {
					aiState = AIState.Patrol;
					animator.SetBool("holding sword", false);
					animator.ResetTrigger("attack");
                }
			} catch {
				Debug.Log ( "Next Waypoint cannot be set due to array indexing issue or array is of length 0 " );
			}
			animator.SetFloat ("vely", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
		} else if (aiState == AIState.Wait) {
			animator.SetFloat ("vely", 0);
		}
		prevVelocity = navMeshAgent.velocity;
	}
	void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "target"){
			aiState = AIState.Patrol;
		}
	}

	bool PlayerInSight()
    {
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		Vector3 origin = transform.position;
		Vector3 direction = transform.forward;

		float maxDistance = 30;

		Debug.DrawRay(origin, direction * 10f, Color.red);
		Ray ray = new Ray(origin, direction);

		if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
		{
			if (hit.transform == player.transform)
			{
				Debug.Log("Enemy can see you!");
				// player is within sight
				return true;
			}
			else
				return false;
		}
		else
			return false;
	}

	bool PlayerInRadius()
    {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		float distance = Vector3.Distance(transform.position, player.transform.position);

		if (distance <= 10f)
		{
			Debug.Log("Player within radius of an enemy.");
			return true;
		}
		else
			return false;
	}

	void attack()
    {
		animator.SetTrigger("attack");
		// reduce the player's health by ___.
	}
}

public enum AIState
{
	Patrol,
	InterceptTarget,
	Wait
};