using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscillateVertically : MonoBehaviour
{
	public float range = 3f;
	public float speed = 1f;

	private float currDiff = 0;
	private bool movingUp = true;

    // Update is called once per frame
    void Update()
    {
    	float delta = Time.deltaTime * speed;

    	if ( currDiff < 0 || currDiff >= range ) movingUp = !movingUp;
        if ( !movingUp ) delta = delta * -1;

        transform.position = transform.position + (Vector3.up * delta);

        currDiff += delta;
    }
}
