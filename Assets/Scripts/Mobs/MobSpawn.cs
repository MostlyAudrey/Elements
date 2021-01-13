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

	public abstract GameObject spawn(Transform transformation);
   
}
