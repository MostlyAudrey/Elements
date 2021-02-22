using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using System.Runtime.Versioning;
using System.ComponentModel.Design;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class MobAI : MonoBehaviour
{
	// Either path or waypoints should be populated but not both, path has priority ofver waypoints
	// A path is the parent game object which the waypoints are childed too
	// waypoints is a list of points to go to
	public GameObject path;
	public GameObject[] waypoints;
	public GameObject target;

	public GameObject swordInHand;
	public GameObject sheathedSword;
	private bool inAttackStance;
	private static UnityEngine.Object[] viking_weapons;

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
		viking_weapons = Resources.LoadAll("Weapons");
		Transform hand = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03/Clavicle_R/Shoulder_R/Elbow_R/Hand_R");
		Debug.Log(hand);
		swordInHand = Instantiate(viking_weapons[0], hand, false) as GameObject;
		swordInHand.transform.localPosition = new Vector3(0.068f, 0.03f, 0.0f);
		swordInHand.transform.localRotation = Quaternion.Euler(-90.0f, 0f, -15f);
		Transform spine = transform.Find("Root/Hips/Spine_01/Spine_02/Spine_03");
		Debug.Log(spine);
		sheathedSword = Instantiate(viking_weapons[0], spine, false) as GameObject;
		sheathedSword.transform.localPosition = new Vector3(-0.172f, 0.175f, -0.282f);
		sheathedSword.transform.localRotation = Quaternion.Euler(-90f, 0f, -138.887f);
		_sheath();
		if (path)
		{
			waypoints = new GameObject[path.transform.childCount];
			for (int i = 0; i < path.transform.childCount; i++)
			{
				waypoints[i] = path.transform.GetChild(i).gameObject;
			}
		}
		if (!target)
			target = GameObject.FindGameObjectWithTag("Player");
		setNextWaypoint();
		navMeshAgent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		aiState = AIState.Patrol;
		velocityReporter = target.GetComponent<VelocityReporter>();

		// sword = (GameObject)Instantiate(Resources.Load("Assets/Downloaded Assets/PolygonVikings/Prefabs/Weapons/SM_Wep_Sword_01.prefab") as GameObject);
		// sword.SetActive(true);
	}

	void Update()
	{
		GameObject player = GameObject.FindGameObjectWithTag("Player");

		if (is_hostile && (PlayerInRadius() || PlayerInSight()))
		{
			Debug.Log("Engaging!");
			_unsheath();
			animator.SetBool("holding sword", true);

			if (aiState == AIState.Patrol)
			{
				aiState = AIState.InterceptTarget;
			}
		}

		if (path && (path.transform.childCount != waypoints.Length))
		{
			waypoints = new GameObject[path.transform.childCount];
			for (int i = 0; i < path.transform.childCount; i++)
			{
				waypoints[i] = path.transform.GetChild(i).gameObject;
			}
		}

		if (aiState == AIState.Patrol)
		{
			if (navMeshAgent.remainingDistance < .5 && !navMeshAgent.pathPending)
			{
				setNextWaypoint();
			}
			animator.SetFloat("vely", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
			animator.SetFloat("velx", (prevVelocity.x - navMeshAgent.velocity.x) / navMeshAgent.speed);
		}
		else if (aiState == AIState.InterceptTarget)
		{
			try
			{
				navMeshAgent.stoppingDistance = 5f;   // stops near the player
				navMeshAgent.SetDestination(target.transform.position + velocityReporter.velocity);

				float distance = Vector3.Distance(transform.position, player.transform.position);
				// if distance between the mob and the player is _____, play the attack animation and reduce the player's health by ____. 
				// (may have to adjust this later, so that the player can counter or dodge or something)
				// also add logic for determining the distance at which the mob attacks (if it is ranged or close)
				if (distance <= 4f)
				{
					_attack();
				}

				// if player moves too far away, put sword away if it is out, and go back to patrolling
				if (distance >= 25f && animator.GetBool("holding sword"))
				{
					aiState = AIState.Patrol;
					animator.SetBool("holding sword", false);
					_sheath();
					animator.ResetTrigger("attack");
				}
			}
			catch
			{
				Debug.Log("Next Waypoint cannot be set due to array indexing issue or array is of length 0 ");
			}
			animator.SetFloat("vely", navMeshAgent.velocity.magnitude / navMeshAgent.speed);
		}
		else if (aiState == AIState.Wait)
		{
			animator.SetFloat("vely", 0);
		}
		prevVelocity = navMeshAgent.velocity;
	}

	public void TakeDamage(int damage)
	{
		Debug.Log("Hit!");
		animator.SetTrigger("hit");
		health -= damage;

		if (health <= 0)
		{
			isDead = true;
			animator.SetTrigger("Dead");
			_die();
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "target")
		{
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

	void _attack()
	{
		if (!inAttackStance)
		{
			animator.SetBool("holding sword", true);
			_unsheath();
		}
		Debug.Log("Enemy attacking...");
		animator.SetTrigger("attack");
		// reduce the player's health by ___.
	}

	void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Hit!");
		if (collision.gameObject.tag == "Player_Weapon")
		{
			animator.SetTrigger("hit");
			health -= 50;
		}
		if (health <= 0)
		{
			_die();
			isDead = true;
			animator.SetTrigger("Dead");
		}
	}

	private void _unsheath()
	{

		swordInHand.SetActive(true);
		sheathedSword.SetActive(false);
	}

	private void _sheath()
	{
		swordInHand.SetActive(false);
		sheathedSword.SetActive(true);
	}

	void _die()
	{
		EventManager.instance.DeathEvent(this);
	}
}

public enum AIState
{
	Patrol,
	InterceptTarget,
	Wait
};