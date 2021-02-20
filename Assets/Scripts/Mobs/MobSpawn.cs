using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityClass {
	WorkerDummySpawn,
	VikingSpawn
}

public abstract class MobSpawn : MonoBehaviour
{

	public string label;
	public EntityClass type;
	public bool is_hostile = false;
	public List<GameObject> spawned_mobs;

	public GameObject path { get; private set;}

	public abstract GameObject spawn(Transform transformation);

	void Awake()
	{
		spawned_mobs = new List<GameObject>();
	}

	public void setPath(GameObject path)
	{
		this.path = path;
		Debug.Log(path);
		foreach (GameObject mob in spawned_mobs)
		{
			mob.GetComponent<MobAI>().path = path;
		}
	}

	public void setHostile()
	{
		this.is_hostile = !this.is_hostile;
		foreach (GameObject mob in spawned_mobs)
		{
			mob.GetComponent<MobAI>().is_hostile = this.is_hostile;
		}
	}

	public void setHostile(bool is_hostile)
	{
		if (this.is_hostile != is_hostile) this.setHostile();
	}
}
