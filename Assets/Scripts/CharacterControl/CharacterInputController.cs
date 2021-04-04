using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour {

    private float filteredMoveFwdInput = 0f;
    private float filteredMoveRightInput = 0f;
    private float filteredTurnInput = 0f;
    private float filteredLookUpInput = 0f;

    public bool InputMapToCircular = true;

    public float moveFwdInputFilter = 5f;
    public float moveRightInputFilter = 5f;
    public float turnInputFilter = 5f;
    public float lookUpInputFilter = 5f;

    private float movementSpeedLimit = 1f;


    public float Forward
    {
        get;
        private set;
    }

    public float Right
    {
        get;
        private set;
    }

    public float Turn
    {
        get;
        private set;
    }

    public float LookUp
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

        

	void Update () 
    {
        //Movement Input
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float moveFwd = Input.GetAxisRaw(GameConstants.k_ButtonNameMoveFwd);
        float moveRight = Input.GetAxisRaw(GameConstants.k_ButtonNameMoveRight);

        if (InputMapToCircular)
        {
            // make coordinates circular
            //based on http://mathproofs.blogspot.com/2005/07/mapping-square-to-circle.html
            moveFwd = moveFwd * Mathf.Sqrt(1f - 0.5f * moveFwd * moveFwd);
            moveRight = moveRight * Mathf.Sqrt(1f - 0.5f * moveRight * moveRight);
        }

        //do some filtering of our input as well as clamp to a speed limit
        filteredMoveFwdInput = Mathf.Clamp(Mathf.Lerp(filteredMoveFwdInput, moveFwd, 
            Time.deltaTime * moveFwdInputFilter), -movementSpeedLimit, movementSpeedLimit);
        filteredMoveRightInput = Mathf.Clamp(Mathf.Lerp(filteredMoveRightInput, moveRight, 
            Time.deltaTime * moveRightInputFilter), -movementSpeedLimit, movementSpeedLimit);

        Forward = filteredMoveFwdInput;
        Right = filteredMoveRightInput;

        //Camera Input
        //GetAxisRaw() so we can do filtering here instead of the InputManager
        float turnDelta = Input.GetAxisRaw(GameConstants.k_ButtonNameTurn);
        float lookUpDelta = Input.GetAxisRaw(GameConstants.k_ButtonNameLookUp);

        //do some filtering of our input as well as clamp to a speed limit
        filteredTurnInput = Mathf.Lerp(filteredTurnInput, turnDelta, 
            Time.deltaTime * turnInputFilter);
        filteredLookUpInput = Mathf.Lerp(filteredLookUpInput, lookUpDelta, 
            Time.deltaTime * lookUpInputFilter);

        Turn = filteredTurnInput;
        LookUp = filteredLookUpInput;

        //Capture "fire" button for action event
        Attack   = Input.GetButtonDown("Fire1");
        Action   = Input.GetKeyDown(KeyCode.Q);
        Jump     = Input.GetKeyDown(KeyCode.Space);
        Interact = Input.GetKeyDown(KeyCode.X);
        Shield   = Input.GetKey(KeyCode.LeftShift);
	}
}
