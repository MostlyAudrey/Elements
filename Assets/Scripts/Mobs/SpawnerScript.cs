using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpawnerScript : MonoBehaviour
{
	// The maximum number of entities to exist at one time
	public int max_spawned_entities = 4;

	// The type of entity to spawn
	public EntityClass entity_class = EntityClass.WorkerDummySpawn;

	// Are the mobs hostile to the player
	public bool is_hostile = false;

    // Minimum time between mob spawns
    public float spawn_delay = 3.0f;

    // Max number of Mobs to Spawn, leave as 0 to continually spawn entities up to max
    public int max_total_spawn = 0;

    // List of all active Mobs spawned by this spawner
    private List<GameObject> current_spawned_entities = new List<GameObject>();

    private float spawn_time_counter = 0.0f;
    private int total_spawns = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Handle spawn timer
        if ( max_total_spawn == 0 || total_spawns < max_total_spawn  ) {
            if ( spawn_time_counter < spawn_delay ) spawn_time_counter += Time.deltaTime;
            else if ( current_spawned_entities.Count < max_spawned_entities ) _spawn_entity(); 
        }       
    }

    public GameObject _spawn_entity() {
        spawn_time_counter = 0.0f;
        total_spawns++;
        MobSpawn mob_spawner = gameObject.AddComponent( Type.GetType( Enum.GetName(typeof(EntityClass), entity_class), true ) ) as MobSpawn;
        mob_spawner.label = String.Concat( Enum.GetName(typeof(EntityClass), entity_class), "_", total_spawns);
        mob_spawner.is_hostile = is_hostile;
        GameObject new_mob = mob_spawner.spawn(transform);
        current_spawned_entities.Add( new_mob );
        return new_mob;
    }
}
