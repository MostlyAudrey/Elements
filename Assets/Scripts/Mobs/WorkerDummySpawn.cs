using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerDummySpawn : MobSpawn
{
	private static UnityEngine.Object[] worker_dummy_objects;

	private static EntityClass type = EntityClass.WorkerDummySpawn;

	bool ready = false;

	public override GameObject spawn(Transform transformation){
		return Instantiate(worker_dummy_objects[Random.Range(0, worker_dummy_objects.Length)], transformation.position, transformation.rotation) as GameObject;
	}

	void Start()
	{
		worker_dummy_objects = new UnityEngine.Object[] { Resources.Load("WorkerDummy1"), Resources.Load("WorkerDummy2") };
		base.type = type;
		this.ready = true;
	}

}
