using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    private Vector3 cloudPos, initCloudPos;
    public float cloudSpeed;

    // Start is called before the first frame update
    void Start()
    {
        initCloudPos = gameObject.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object forward along its -x axis 1 unit/second.
        transform.Translate(cloudSpeed*Vector3.left * Time.deltaTime);
    }

    void OnTriggerEnter(Collider cloudCollider)
    {
        Debug.Log("Another object has entered the trigger");
        if (cloudCollider.CompareTag("CloudEnd"))
        {
            Debug.Log("Collision!");
            gameObject.transform.position = initCloudPos;
        }
    }
}
