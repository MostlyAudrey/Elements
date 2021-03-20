using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;
using FMODUnity;

#if UNITY_EDITOR
using UnityEditor;
#endif

//require some things the bot control needs
[RequireComponent(typeof(Animator), typeof(Rigidbody), typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterInputController))]
public class RootMotionControlScript : MonoBehaviour
{
    private Animator anim;	
    private Rigidbody rbody;
    private CharacterInputController cinput;

    private Transform leftFoot;
    private Transform rightFoot;

    //Useful if you implement jump in the future...
    public float jumpableGroundNormalMaxAngle = 45f;
    public bool closeToJumpableGround;

	public float animationSpeed = 1f;
	public float rootMovementSpeed = 1f;
	public float rootTurnSpeed = 1f;
    public float fallSpeed = 1f;
    public float buttonRadius = 5f;

    public float pickupSpeed = 1f;
    public float stepDown = 0.3f;

    public GameObject pickedUpItem;

	private GameObject buttonObject;

    public GameObject HoldSpot;

    public GameObject swordInHand;

    public GameObject sheathedSword;

    public bool isGrounded;

    public bool hasSword;

    private bool inAttackStance;

    private FMOD.Studio.EventInstance buttonAudio;

    private CharacterAudio characterAudio;

    // Use this for initialization
    void Awake()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
            Debug.Log("Animator could not be found");

        rbody = GetComponent<Rigidbody>();

        if (rbody == null)
            Debug.Log("Rigid body could not be found");

        cinput = GetComponent<CharacterInputController>();
        if (cinput == null)
            Debug.Log("CharacterInput could not be found");

        anim.applyRootMotion = true;

		//example of how to get access to certain limbs
        // leftFoot = this.transform.Find("Root/Hips/UpperLeg_L/LowerLeg_L/Ankle_L");
        // rightFoot = this.transform.Find("Root/Hips/UpperLeg_R/LowerLeg_R/Ankle_R");

        // if (leftFoot == null || rightFoot == null)
        //     Debug.Log("One of the feet could not be found");

        isGrounded = false;

        //never sleep so that OnCollisionStay() always reports for ground check
        rbody.sleepThreshold = 0f;
    }

    void Start()
    {
        hasSword = (QuestManager.GetQuestPhase( QuestName.PerformDiagnostics ) == 3);
        sheath();

        buttonAudio = RuntimeManager.CreateInstance("event:/Interactables/Button");

        characterAudio = GetComponent<CharacterAudio>();
    }
    
    private bool debounceInteractButton = false;
    private bool debounceActionButton = false;
    private bool debounceAttackButton = false;
    private bool debounceJumpButton = false;
    private bool itemInPosition = false;

    void Update()
    {
        // // Event-based inputs need to be handled in Update()

        if(cinput.Interact && !debounceInteractButton )
        {
            sheath();
            EventManager.instance.ActionButtonPressed();
            _checkForButton();
            debounceInteractButton = true; 
        } 
        else if (!cinput.Interact && debounceInteractButton)
            debounceInteractButton = false;

        if(cinput.Attack && !debounceAttackButton )
        {
            _attack();
            debounceAttackButton = true; 
        } 
        else if (!cinput.Attack && debounceAttackButton)
            debounceAttackButton = false;

        
        if(cinput.Jump && !debounceJumpButton )
        {
            _jump();
            debounceJumpButton = true; 
        } 
        else if (!cinput.Jump && debounceJumpButton)
            debounceJumpButton = false;


        if ( cinput.Action && !debounceActionButton)
        {
            if ( pickedUpItem ) _putdown_item();
            else 
            {
                GameObject target = CharacterCommon.CheckForNearestPickupableItem(transform, 100f);
                if (target) _pickup_item(target);
            }
            debounceActionButton = true; 
        }
        else if (!cinput.Action && debounceActionButton)
            debounceActionButton = false;

        if ( pickedUpItem )
        {
            if ( !itemInPosition && Helper.WithinRadius(pickedUpItem.transform.position, HoldSpot.transform.position, .5f) ) itemInPosition = true;
            if ( !itemInPosition ) pickedUpItem.transform.position += (HoldSpot.transform.position - pickedUpItem.transform.position) * (Time.deltaTime * pickupSpeed);
            else pickedUpItem.transform.position = HoldSpot.transform.position;
            
            if ( itemInPosition && pickedUpItem.GetComponent<ItemPickupTrigger>() ) pickedUpItem.GetComponent<ItemPickupTrigger>().inPosition();
        } 

		anim.speed = animationSpeed;
    }

    void FixedUpdate()
    {

        float inputForward=0f;
        float inputTurn=0f;

        // input is polled in the Update() step, not FixedUpdate()
        // Therefore, you should ONLY use input state that is NOT event-based in FixedUpdate()
        // Input events should be handled in Update(), and possibly passed on to FixedUpdate() through 
        // the state of the MonoBehavior
        if (cinput.enabled)
        {
            inputForward = cinput.Forward;
            inputTurn = cinput.Turn;
        }

        int surface;
	
        //onCollisionStay() doesn't always work for checking if the character is grounded from a playability perspective
        //Uneven terrain can cause the player to become technically airborne, but so close the player thinks they're touching ground.
        //Therefore, an additional raycast approach is used to check for close ground
        // anim.SetBool("isFalling", !CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, 1f, 1f, out closeToJumpableGround));
        if (CharacterCommon.CheckGroundNear(this.transform.position, jumpableGroundNormalMaxAngle, .2f, 1f, out closeToJumpableGround, out surface))
        {
            isGrounded = true;
        }
        
        characterAudio.surface = surface;
       
        anim.SetFloat("velx", inputTurn);	
        anim.SetFloat("vely", inputForward);
    }

    //This is a physics callback
    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;

    }

    //This is a physics callback
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.gameObject.tag == "ground")
        {
      
            // EventManager.TriggerEvent<PlayerLandsEvent, Vector3, float>(collision.contacts[0].point, collision.impulse.magnitude);

        }
						
    }

    void OnAnimatorMove()
    {

        Vector3 newRootPosition;
        Quaternion newRootRotation;

        if (isGrounded)
        {
         	//use root motion as is if on the ground		
            newRootPosition = anim.rootPosition;
        }
        else
        {
            //Simple trick to keep model from climbing other rigidbodies that aren't the ground  - Time.deltaTime * fallSpeed
            newRootPosition = anim.rootPosition + ( Vector3.down * Time.deltaTime * stepDown ); //new Vector3(anim.rootPosition.x, this.transform.position.y, anim.rootPosition.z);
        }

        //use rotational root motion as is
        newRootRotation = anim.rootRotation;


		this.transform.position = Vector3.LerpUnclamped( this.transform.position, newRootPosition, rootMovementSpeed );
		this.transform.rotation = Quaternion.LerpUnclamped( this.transform.rotation, newRootRotation, rootTurnSpeed );
			
        //clear IsGrounded
        isGrounded = false;
    }

	void OnAnimatorIK()
	{
		if (anim) {
			AnimatorStateInfo astate = anim.GetCurrentAnimatorStateInfo (0);

			if (astate.IsName ("ButtonPress")) {
				float buttonWeight = anim.GetFloat ("buttonClose");
                // Debug.Log(buttonObject.transform.position);
				if (buttonObject) {
					anim.SetLookAtWeight (buttonWeight);
					anim.SetLookAtPosition (buttonObject.transform.position + (Vector3.down * 1.35f));
					anim.SetIKPositionWeight (AvatarIKGoal.RightHand, buttonWeight);
					anim.SetIKPosition (AvatarIKGoal.RightHand, buttonObject.transform.position + (Vector3.down * 1.35f));
				}
			} else {
				anim.SetIKPositionWeight (AvatarIKGoal.RightHand, 0);
				anim.SetLookAtWeight (0);
			}
		}
	}

    private void _pickup_item(GameObject target)
    {
        pickedUpItem = target;
        pickedUpItem.GetComponent<Rigidbody>().isKinematic = true;
        pickedUpItem.transform.parent = transform;
        itemInPosition = false;
    }

    private void _putdown_item()
    {
        pickedUpItem.GetComponent<Rigidbody>().isKinematic = false;
        pickedUpItem.transform.parent = null;
        pickedUpItem = null;
        itemInPosition = false;
    }

    public void destroy_picked_up_item()
    {
        Destroy(pickedUpItem);
        itemInPosition = false;
        pickedUpItem = null;
    }

    private void _attack()
    {
        if (!inAttackStance)
            _unsheath();
        anim.SetTrigger("attack");
    }

    private void _jump()
    {
        anim.SetTrigger("jump");
    }

    private void _unsheath()
    {
        inAttackStance = hasSword;
        anim.SetBool("holding sword", hasSword);
        swordInHand.SetActive(hasSword);
        sheathedSword.SetActive(false);
    }

    public void sheath()
    {
        inAttackStance = false;
        anim.SetBool("holding sword", false);
        swordInHand.SetActive(false);
        sheathedSword.SetActive(hasSword);
    }

    private void _checkForButton()
    {
        foreach( Collider collision in Physics.OverlapSphere(transform.position, buttonRadius) )
        {      
            if ( collision.gameObject.tag == "Button" )
            {
                buttonObject = collision.gameObject;
                this.anim.SetTrigger("buttonPress");
                return;
            }
        }
    }

    public void buttonPushed()
    {
        buttonObject.GetComponent<ButtonPressTrigger>().pushButton();
        buttonAudio.start();
    }

    public void LoadLocation(PlayerData data)
    {
        Quaternion currRot = transform.rotation;
        Vector3 newLoc = data.GetPlayerLocation();
        transform.SetPositionAndRotation(newLoc, currRot);
        Debug.Log("Loaded player location at " + newLoc);
    }
    
}
