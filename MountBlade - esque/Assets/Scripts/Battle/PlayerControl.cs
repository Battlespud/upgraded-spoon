using UnityEngine;
using System.Collections;

public enum WeaponType{
	MELEE,
	RANGED
};


//very basic very shit player controller.
public class PlayerControl : MonoBehaviour
{
    #region Component Variables
    Rigidbody rigidBody;
    Animator anim;
    CapsuleCollider capCol;
    [SerializeField] PhysicMaterial zfriction; //we want zero friction when we move
    [SerializeField] PhysicMaterial mfriction; //but maximum friction when we are stationary
    Transform cam;
    CharacterStats charStats;
    #endregion

	float baseSpeed = .3f;

    float speed = .3f; //How fast we move
    [SerializeField] float turnSpeed = 15; //How fast we turn

	//weapon stuff
	public WeaponType weaponType;
	public GameObject PistolIKPoint;
	public GameObject NoPistolIKPoint;
	public GameObject Muzzle;
	public Weapon weapon;

	Vector3 camDefaultPosition;
	Vector3 camOverShoulderPosition;

	float defaultFov = 90f;
	float sightsFov = 45f;

	bool WeaponDrawn = false;
	bool lockoutDraw = false;
	bool AimingSights =false;
	bool lockoutAim = false;

	bool sprinting = false;


    Vector3 directionPos; //The direction we look at
    Vector3 lookPos; //Where we look at, used in IK
    Vector3 curLookPos;
    float bodyWeight = .1f; //This controls how much the body will turn to look at the position

    //Input Variables
    float horizontal;
    float vertical;  
    float MouseX;
    float MouseY;

    #region Attack Variables
    //Attack Variables 
    [SerializeField] float lerpRate = 5; //How fast the attack animation plays
    [SerializeField] float attackTimer = 1; //For how much time will we be attacking
    
    //Internal Variables
    float targetValue; //the target value of the attack state, 0 means we hold the attack, 1 means we have striked
    float curValue;   //the current value of the attack state
    float aTimer;
    float decTimer; //see below for more info
    bool holdAttack; //if we are holding an attack
    public bool attack; //if we are attacking
    #endregion

    #region Block Variables
    bool blocking; //if we are blocking
    float bTimer; //block timer that controls how fast we recover from a blocked attack
    public bool blockedAttack; //if our attack was blocked
    #endregion

	// Use this for initialization
	void Start () 
    {
        //Setup the refferences
		camOverShoulderPosition= new Vector3(1f, camDefaultPosition.y, -2.5f);
        rigidBody = GetComponent<Rigidbody>();
        cam = Camera.main.transform;
        capCol = GetComponent<CapsuleCollider>();
        SetupAnimator();
        charStats = GetComponent<CharacterStats>();
		Chest = anim.GetBoneTransform (HumanBodyBones.Chest);
		if(GetComponentInChildren<Weapon>()){
			weaponType = WeaponType.RANGED;
			weapon = GetComponentInChildren<Weapon> ();
		}
		else{
			weaponType = WeaponType.MELEE;
			}
			
	}
	
	void Update () 
    {
		if (Input.GetKey (KeyCode.LeftShift)) {
			speed = 1.5f*baseSpeed;
			sprinting = true;
		} else {
			speed = baseSpeed;
			sprinting = false;
		}
		if (Input.GetKeyDown (KeyCode.G)) {
			SwitchWeaponTypes();
		}
		if (weaponType == WeaponType.RANGED) {
			WeaponInput ();
		}
        //We do a ray from the camera to see where the IK will look
        Ray ray = new Ray(cam.position, cam.forward);

        //Before updating the look position, we find the angle between the forward of our transform and the ray.point
        Vector3 dir = ray.GetPoint(100) - transform.position;
        float angle = Vector3.Angle(transform.forward, dir);

        //We update the look position only if the angle is lower than 50o, you can play with this to get your desired effect
        if (angle < 50)
        lookPos = ray.GetPoint(100);

        //Lerp the look position with the current look position
        curLookPos = Vector3.Lerp(curLookPos, lookPos, Time.deltaTime * 15);

        HandleFriction();
		if (weaponType == WeaponType.MELEE) {
			ControlAttackAnimations ();
			ControlBlockAnimations ();
		} else {

		}

		anim.SetFloat ("Velocity", rigidBody.velocity.magnitude);
//		Debug.Log (rigidBody.velocity.magnitude);
	}

    void FixedUpdate()
    {
        //We want to move the character only when he is not on a mount
        if (!charStats.riding)
        {
            bodyWeight = .5f; //and set the bodyweight to what we previously used

            //Our Inputs
            horizontal = Input.GetAxis("Vertical");
            vertical = Input.GetAxis("Horizontal");

            //We apply forces, either to the x axis or to the z of our transform, 
            //if you want to jump you can add an extra vector with transform.up
            rigidBody.AddForce(((transform.right * vertical) + (transform.forward * horizontal)).normalized * speed / Time.deltaTime);

            //Find a position in front of where the camera is looking
            directionPos = transform.position + cam.forward * 100;
            //Find the direction from that position
            Vector3 dir = directionPos - transform.position;
            dir.y = 0;//kill the y

            //Then look towards that direction with .Slerp()
            rigidBody.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);

            //Pass the values to the animator
            anim.SetFloat("Forward", horizontal, 0.1f, Time.deltaTime);
            anim.SetFloat("Sideways", vertical, .1f, Time.deltaTime);
        }     
    }

    void HandleFriction()
    {
        //If we there's no input
        if(horizontal == 0 && vertical == 0)
        {
            //we probably are stationary so we want maximum friction, 
            capCol.material = mfriction;
            //we should probably add a check that looks if we are on the ground or not also
        }
        else
        {
            //if we are moving then we don't want any friction
            capCol.material = zfriction;
        }
    }

    void ControlAttackAnimations()
    {
        //Get the inputs from the mouse
        MouseX = Input.GetAxis("Mouse X");
        MouseY = Input.GetAxis("Mouse Y");

        //get the axis for the left mouse button
        float mousLB = Input.GetAxis("Fire1");

        #region Decide Attack Type
        if (mousLB > 0.1f) //if the axis is greater than 0, that means the player is pressing the left mouse button
        {
            decTimer += Time.deltaTime; //Start counting how much time the player has to move the mouse to decide the attack animation

            int attackType = 0; //the init integer

            //We check which value is higher, so where did the player move the mouse the most (*the most is a key word here!)
            //..and then change the attack type depending on the case and what we have set in the Animator Controller
            if(Mathf.Abs(MouseX) > Mathf.Abs(MouseY))
            {
                if(MouseX < 0) 
                {
                    attackType = 1; 
                }
                else
                {
                    attackType = 1;
                }
            }
            else
            {
                attackType = 0;
            }

            //Pass the attack type to the Animator
            anim.SetInteger("AttackType", attackType);

            //After the window to decide closes
            if(decTimer > .4f)
            {
                //Then we start holding the attack
                holdAttack = true;
                //We pass it to the animator
                anim.SetBool("Attack", true);
                //We reset the timer
                decTimer = 0;
            }
        }
        else
        {
            anim.SetBool("Attack", false);
        }
        #endregion
     
    }

    void ControlBlockAnimations()
    {
       if(blockedAttack)
       {
           anim.SetTrigger("Blocked");
           blockedAttack = false;
       }

       float rightClick = Input.GetAxis("Fire2");

        if(rightClick > 0.1f)
        {
            anim.SetBool("Block", true);
        }
        else
        {
            anim.SetBool("Block", false);
        }
    }


    void SetupAnimator()
    {
        // this is a ref to the animator component on the root.
        anim = GetComponent<Animator>();

        // we use avatar from a child animator component if present
        // this is to enable easy swapping of the character model as a child node
        foreach (var childAnimator in GetComponentsInChildren<Animator>())
        {
            if (childAnimator != anim)
            {
                anim.avatar = childAnimator.avatar;
                Destroy(childAnimator);
                break; //if you find the first animator, stop searching
            }
        }
    }
  
	float LeftHandIKWeight;

	float LookIKWeight;

	Transform Chest;


    void OnAnimatorIK()
    {
        //Simple stuff, the values here work pretty good for me as seen in the video
        anim.SetLookAtWeight(1, bodyWeight, 1, 1, 1);
        anim.SetLookAtPosition(curLookPos);

        if(charStats.riding)//Get the IK positions from the ones we stored at the horse controller
        { 
            HorseControlPlayer.RiderIKpositions rIKpos = charStats.mount.GetComponent<HorseControlPlayer>().riderIKpositions;

            float riding = anim.GetFloat("RidingIKfloat");

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, riding);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, rIKpos.LeftHand.position);

            anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, riding);
            anim.SetIKPosition(AvatarIKGoal.RightFoot, rIKpos.RightFoot.position);

            anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, riding);
            anim.SetIKPosition(AvatarIKGoal.LeftFoot, rIKpos.LeftFoot.position);
            
        }

		//For aiming if we have a gun
		if (weaponType == WeaponType.RANGED) {
			LeftHandIKWeight = Mathf.Lerp (LeftHandIKWeight, 1f, .045f * Time.deltaTime);
			float f = curLookPos.y;
			if (f < .75f)
				f = .75f;
			else if (f > 1.95f)
				f = 1.95f;
			anim.SetIKPosition (AvatarIKGoal.LeftHand, new Vector3(PistolIKPoint.transform.position.x, f, PistolIKPoint.transform.position.z));
			anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, LeftHandIKWeight);
		}
		else if (weaponType == WeaponType.MELEE && LeftHandIKWeight !=1f) {
			LeftHandIKWeight = Mathf.Lerp (LeftHandIKWeight, 1f, .045f * Time.deltaTime);
			anim.SetIKPosition (AvatarIKGoal.LeftHand, NoPistolIKPoint.transform.position);
			anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, LeftHandIKWeight);
		}
		else if (weaponType == WeaponType.MELEE && LeftHandIKWeight != 1f) {
		}
    }

	void SwitchWeaponTypes(){
		if (weaponType == WeaponType.MELEE)
		{	weaponType = WeaponType.RANGED;
		}
		else	if (weaponType == WeaponType.RANGED){
			weaponType = WeaponType.MELEE;
	}
		LeftHandIKWeight = 0f;
}



	//Weapon Stuff
	void ToggleSights(){
		if (!AimingSights && !lockoutAim && (WeaponDrawn || !lockoutDraw)) {
			AimingSights = true;
			if (!WeaponDrawn) {
				ToggleHolster ();
			}
			lockoutAim = true;
			StartCoroutine(ShiftFov(sightsFov,.5f));
		}  else if(!lockoutAim) {
			AimingSights = false;
			lockoutAim = true;
			StartCoroutine(ShiftFov(defaultFov,.5f));
		}
	}


	void ToggleCursorVisible(){
		if (Cursor.visible) {
			Cursor.visible = false;
			Cursor.lockState = CursorLockMode.Locked;
		}  else {
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Locked;
		}
	}

	private IEnumerator ShiftPosition(Vector3 finish, float time){
		float elapsedTime = 0f;
		while (elapsedTime < time) {
			cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition,finish,(elapsedTime/time));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		lockoutDraw = false;
	}

	void ToggleHolster(){
		if (WeaponDrawn && !lockoutDraw) {
			WeaponDrawn = false;
		//	ToggleCursorVisible ();
			lockoutDraw = true;
			StartCoroutine (ShiftPosition (camDefaultPosition, .5f));
		}  else if(!lockoutDraw) {
			WeaponDrawn = true;
		//	ToggleCursorVisible ();
			lockoutDraw = true;
			StartCoroutine (ShiftPosition (camOverShoulderPosition, .5f));
		}
	}

	private IEnumerator ShiftFov(float finish, float time){
		float elapsedTime = 0f;
		while (elapsedTime < time) {
			Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView,finish,(elapsedTime/time));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		lockoutAim = false;
	}

	void WeaponInput(){
		if (Input.GetKeyDown (KeyCode.R) && !sprinting) 
			weapon.Reload ();
		if (Input.GetMouseButtonDown (0)) {
			if (!weapon.automatic) {
				if (WeaponDrawn && !weapon.automatic) {
					weapon.Fire ();
				} else {
					ToggleHolster ();
				}
			}
		}
		if (Input.GetMouseButton (0) && !sprinting) {
			if (weapon.automatic) {
				if (WeaponDrawn) {
					weapon.Fire ();
				} else {
					ToggleHolster ();
				}
			}
		}
		if (Input.GetMouseButtonDown(1) &&!sprinting)
			ToggleSights();
		
		if (Input.GetKeyDown(KeyCode.H) &&!sprinting)
			ToggleHolster();
	}


	public bool inAttackAnimation;

	//Animation
	public void AnimationStarted(){
		inAttackAnimation = true;
	}

	public void AnimationEnded(){
		inAttackAnimation = false;
	}


}
