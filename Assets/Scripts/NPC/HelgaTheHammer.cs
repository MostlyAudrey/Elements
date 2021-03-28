using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelgaTheHammer : MonoBehaviour
{
    private WalkToTarget walkScript;

    // Start is called before the first frame update
    void Start()
    {
        walkScript.enabled = false;
        walkScript = gameObject.GetComponent<WalkToTarget>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            // do all the things
            walkScript.enabled = true;
        }
    }
}
