using UnityEngine;
using System.Collections;

public class HorseControlPlayer : MonoBehaviour
{
    #region Component Variables
    Rigidbody rigidBody;
    Animator anim;
    #endregion

    //If it has a rider basically and that rider is a player
    public bool playerControlled;

    [SerializeField] float speed; //How fast we move
    [SerializeField]
    float actualMaxSpeed = 5; //How fast we move
    [SerializeField] float turnSpeed = 1; //How fast we turn
    [SerializeField]
    float maxAngularDrag = 30;
    [SerializeField]
    float minAngularDrag;

    //Input Variables
    float horizontal;
    float vertical;

    public Transform riderPositionTransform;//This is the parent of the rider, place the bone you want him to sit upon
    public bool riderAtLeftSide;//If the rider is at the left side before he gets up on the horse

    float curDistance;//The current distance the horse has from the ground
    public float heightPos = 1; //The offset from the ground

    [SerializeField]
    public RiderIKpositions riderIKpositions;
    [System.Serializable]
    public class RiderIKpositions
    {
        //Populate this variables with empty gameobjects that will serve as IK targets
        public Transform LeftHand;
        public Transform LeftFoot;
        public Transform RightFoot;
    }

	// Use this for initialization
	void Start () 
    {
        //Setup the refferences
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.drag = 5;
        rigidBody.angularDrag = 25;//Change this if it doesn't work for you
        minAngularDrag = rigidBody.angularDrag;

        SetupAnimator();
        speed = 0;
	}
	
    void FixedUpdate()
    {
        ControlPositions();

        if (playerControlled)//If it's controlled by the player
        {
            //Our Inputs
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if(vertical > 0)
            {
                //Then for as long as the player it holds the W key, we add to the speed
                speed += Time.deltaTime / 2;
                //The faster we are going, the harder it is to turn
                rigidBody.angularDrag += Time.deltaTime;
            }
            else if( vertical != 0)
            {
                //If he is holding the S key, we substract from the speed
                speed -= Time.deltaTime / 2;
                //and since we are going slower we can turn faster
                rigidBody.angularDrag -= Time.deltaTime;
            }

            //A simple function that controls the minimu and maximum of the values so everything would work normally
            ControlMaxMin();      
        }
        else //If there's no rider
        {
            //Then smoothly stop moving
            speed -= Time.deltaTime;      
     
            if(speed < 0)
            {
                speed = 0;
            }
        }

        //If there's a wall ahead
        bool blockedMovement = MovementObstruct();

        if (blockedMovement)
            speed = 0;//stop moving

        //Pass the speed to the rigidbody
        rigidBody.AddForce(transform.forward * actualMaxSpeed * speed / Time.deltaTime);

        //and add torque in case we need to turn
        rigidBody.AddTorque((transform.up * horizontal) * turnSpeed / Time.deltaTime);

        //Store the speed as a new float
        float animSpeed = speed;
        //that we manipulate to help the blendtree with the animations
        if (animSpeed > 0 && animSpeed < 0.5f)
        {
            animSpeed = 0.5f;
            if (blockedMovement)
                animSpeed = 0;
        }

        anim.SetFloat("Forward", animSpeed, 0.1f, Time.deltaTime);
    }

    void ControlMaxMin()
    {
        if (speed > 1)
        {
            speed = 1;
        }

        if (speed < 0)
        {
            speed = 0;
        }

        if (rigidBody.angularDrag < minAngularDrag)
        {
            rigidBody.angularDrag = minAngularDrag;
        }

        if (rigidBody.angularDrag > maxAngularDrag)
        {
            rigidBody.angularDrag = maxAngularDrag;
        }
    }


    void ControlPositions()
    {
        //Do a raycast to ground and keep the box collider to that distance from the ground
        //we avoid micro collisions this way, especially with annoying and uneven mesh colliders
        RaycastHit hitPos;
        if(Physics.Raycast(transform.position + transform.up, -transform.up, out hitPos, 100))
        {
            curDistance = Vector3.Distance(transform.position + transform.up, hitPos.point);
        }

        //if the distance is not as it should be
        if (curDistance != heightPos)
        {
            //find the difference
            float difference = heightPos - curDistance;
            //and interpolate our position to the new one with the difference added on our Y axis
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(0, difference, 0), Time.deltaTime * 5);
        }
    }

    //Do a raycast in the front to see if there's any obstacle and return true if there is
    bool MovementObstruct()
    {
        bool retVal = false;

        RaycastHit hit;
        Debug.DrawRay(transform.position + transform.up, transform.forward * 1.5f);
        if(Physics.Raycast(transform.position + transform.up,transform.forward, out hit,1.5f))
        {
            //You can add here more logic, for example if you want to trample somebody, this would be a good place to add that
            retVal = true;
        }

        return retVal;
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
}
