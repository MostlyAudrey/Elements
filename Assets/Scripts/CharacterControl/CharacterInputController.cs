﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour {

    private float filteredForwardInput = 0f;
    private float filteredTurnInput = 0f;

    public bool InputMapToCircular = true;

    public float forwardInputFilter = 5f;
    public float turnInputFilter = 5f;

    private float forwardSpeedLimit = 1f;


    public float Forward
    {
        get;
        private set;
    }

    public float Turn
    {
        get;
        private set;
    }

    public bool Action
    {
        get;
        private set;
    }

    public bool Attack
    {
        get;
        private set;
    }

    public bool Jump
    {
        get;
        private set;
    }

	public bool FollowTarget
	{
		get;
		private set;
	}

	public bool Interact
	{
		get;
		private set;
	}

    public bool Shield
	{
		get;
		private set;
	}

        

	void Update () {
		
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float h = Input.GetAxisRaw("Horizontal");// setup h variable as our horizontal input axis
        float v = Input.GetAxisRaw("Vertical"); // setup v variables as our vertical input axis


        if (InputMapToCircular)
        {
            // make coordinates circular
            //based on http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html
            h = h * Mathf.Sqrt(1f - 0.5f * v * v);
            v = v * Mathf.Sqrt(1f - 0.5f * h * h);

        }

        //do some filtering of our input as well as clamp to a speed limit
        filteredForwardInput = Mathf.Clamp(Mathf.Lerp(filteredForwardInput, v, 
            Time.deltaTime * forwardInputFilter), -forwardSpeedLimit, forwardSpeedLimit);

        filteredTurnInput = Mathf.Lerp(filteredTurnInput, h, 
            Time.deltaTime * turnInputFilter);

        Forward = filteredForwardInput;
        Turn = filteredTurnInput;


        //Capture "fire" button for action event
        Attack   = Input.GetButtonDown("Fire1");
        Action   = Input.GetKeyDown(KeyCode.Q);
        Jump     = Input.GetKeyDown(KeyCode.Space);
        Interact = Input.GetKeyDown(KeyCode.X);
        Shield   = Input.GetKey(KeyCode.LeftShift);

	}
}
