using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyControl : MonoBehaviour {

    [SerializeField] float turnSpeed = 2;

    [SerializeField]
    float attackLerpRate = 5; //How fast the animations are blended together
    [SerializeField]
    float attackRate = 3; //How soon will he attack
    [SerializeField]
    float attackDuration;
   
    public bool blockedAttack; //If the attack was blocked
    
    bool decideAttack; //If he decided between the 3 attacks
    public bool attacking;//If he is attacking
    float attackTimer;
    float targetValue;
    float curValue;

    public bool blocking;//If he is blocking
    float blockTimer;

	//weapon stuff
	Weapon weapon;
	float LeftHandIKWeight;

    //Our component variables
    CharacterStats charStats;
    UnityEngine.AI.NavMeshAgent agent;
    Rigidbody rigidBody;
    Animator anim;
    GameManager gm;
    Transform CurrentAttackingEnemy;
    CharacterStats enemStats;

    //Our list with the current enemies
    List<Transform> currentEnemies = new List<Transform>();

    //We use this with the look IK
    Vector3 lookingPosition;

    //The states our AI can have
    public enum AIState
    {
        Charge,
        Hold,
        Follow
    }

    //By default they are on Charge
    public AIState aiState = AIState.Charge;

    public Transform generalCharacter; //Which transform should they follow
    public Vector3 holdPosition; //Where should they hold position


	//MIRRORING FOR AIR BATTLES
	public bool inMirrorMode;
	public EnemyControlProxy proxy;


	// Use this for initialization
	void Start () 
    {
        agent = GetComponentInChildren<UnityEngine.AI.NavMeshAgent>();
        rigidBody = GetComponentInChildren<Rigidbody>();
        charStats = GetComponent<CharacterStats>();
        SetupAnimator();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		weapon = GetComponentInChildren<Weapon> ();
       

        //We want the stopping distance of the navmesh agent to be the same as our attack range
        agent.stoppingDistance = charStats.AttackRange; 
	}
	
	void Update () 
    {
        //Call the functions based on the AI state
        switch(aiState)
        {
            case AIState.Charge:
                 FindTarget();
                 MoveToTarget();
                 CheckCurrentEnemy();
                break;
            case AIState.Follow:
                FollowGeneral();
                break;
            case AIState.Hold:
                HoldPosition();
                break;
        }
		if (inMirrorMode) {
			proxy.MirrorMove (agent.desiredVelocity);
			proxy.SetRotation (transform.rotation);
//			Debug.Log (agent.desiredVelocity);
		}
       
	}

    void FollowGeneral()
    {
        //This state simple makes the ai character to follow a target
        CurrentAttackingEnemy = null;
        generalCharacter = GameObject.FindGameObjectWithTag("Player").transform;
        agent.SetDestination(generalCharacter.position);
        anim.SetFloat("Sideways", Mathf.Abs(agent.desiredVelocity.z), 0.1f, Time.deltaTime);
    }

    void HoldPosition()
    {
        //this states simply makes the ai character to go to a position
        CurrentAttackingEnemy = null;
        agent.SetDestination(holdPosition);
        anim.SetFloat("Forward", Mathf.Abs(agent.desiredVelocity.z), 0.1f, Time.deltaTime);
    }

    void MoveToTarget()
    {
        //If we have an enemy
        if(CurrentAttackingEnemy)
        {

			float distance = Vector3.Distance(transform.position, CurrentAttackingEnemy.position);

            //Then we have a destination
			if (weapon == null || distance > .6f * weapon.maxRange) {
				agent.SetDestination (CurrentAttackingEnemy.position);
			}
			if (weapon != null && distance <= .6f * weapon.maxRange) {
				agent.isStopped = true;
			}
            //Since we know that the agent can move, we know that we can use the desired velocity to play the walking animation
            //but we want to use only the positive value of z (for now)
            float movement = Mathf.Abs(agent.desiredVelocity.z) + Mathf.Abs(agent.desiredVelocity.x) + Mathf.Abs(agent.desiredVelocity.y);

            anim.SetFloat("Forward", Mathf.Abs(agent.desiredVelocity.z),0.1f,Time.deltaTime);

            //Find the distance between the target

			if (weapon != null) {
				if (distance < charStats.AttackRange) {
					weapon = null;
				} else if (distance < .8f * weapon.maxRange) {
					RaycastHit hit;
					weapon.Fire ();		
					if (weapon.ammo <= 0)
						weapon.Reload ();
				}
			}
            //..and if we are close and before stopping,
            if(distance < charStats.AttackRange + 2)
            {
                //Start looking towards the enemy
                Vector3 dir = CurrentAttackingEnemy.position - transform.position;
                dir.y = 0;

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), turnSpeed * Time.deltaTime);

                //And if he is not attacking
				if (!IsEnemyAttacking ())
				if (weapon != null) {
					weapon.Fire ();
					if (weapon.ammo <= 0)
						weapon.Reload ();
				}
				else{
                    Attacking(); //Attack
				}
                else// or block
					if (weapon == null) 
                    Blocking();
            }
        }
    }

    void Attacking()
    {
        //You are not blocking if you are attacking
        anim.SetBool("Block", false);

        if (!attacking) //If you are not attacking
        {
            if (!decideAttack)//and haven't decided an attack yet
            {            
                 attackTimer += Time.deltaTime;

                 if (attackTimer > attackRate)
                 {
                     //Decide one at random, you can add further criteria here in decided which type of attack you want
                     //For instance checking if the enemy blocks and deciding on a counter attack of that
                     int ran = Random.Range(0, 2 + 1);
                     anim.SetInteger("AttackType", ran);

                     anim.SetBool("Attack", true);
                     decideAttack = true;
                     attackTimer = 0;
                 }
            }
            else
            {
                //If you have decided an attack, then attack 
                attackTimer += Time.deltaTime;

                if (attackTimer > attackRate)
                {
                    attacking = true;
                    attackTimer = 0;
                }
            }
        }
        else
        {
            //If you are attacking
            attackDuration += Time.deltaTime;

         
            
            if(attackDuration > 1)
            {
                anim.SetBool("Attack", false);
                //You are attacking
                attacking = false;
                attackDuration = 0;
                decideAttack = false;    
                blockedAttack = false;
            }
        }

        if (blockedAttack) //but if the attack is blocked
        {
            targetValue = 0; //the target value of the blend tree, is at 0
        }
    }

    void Blocking()
    {
        //If you are blocking
        //Then see what attack the enemy is doing
        int blockType = CurrentAttackingEnemy.GetComponent<Animator>().GetInteger("AttackType");
        //And block accordingly
        //anim.SetFloat("BlockSide", blockType);
        anim.SetBool("Block", true);

        //and basically reset your attacks
        attacking = false;
        attackDuration = 0;
        anim.SetBool("Attack", false);
        decideAttack = false;
        targetValue = 0;
        blockedAttack = false;
    }

    void FindTarget()
    {
        //If you don't have an enemy
        if(CurrentAttackingEnemy == null)
        {
           //Then check the list of the current Players
           foreach(Transform tran in gm.CurrentPlayers)
           {
               //if a transform's character id is not the same as ours
               if(tran.GetComponent<CharacterStats>().characterID != charStats.characterID)
               {
                   //and we haven't already add him to our list of enemies, then add him
                   if(!currentEnemies.Contains(tran))
                   {
                       currentEnemies.Add(tran);
                   }
               }
           }

           if (currentEnemies.Count > 0)
           {
               //And decide an enemy at random
               int ran = Random.Range(0, currentEnemies.Count - 1);

               CurrentAttackingEnemy = currentEnemies[ran];
           }
           else
           {
               holdPosition = transform.position;
               aiState = AIState.Hold;
           }

            //You can add different criteria here on deciding who to attack, we have done something like this in previous videos
        }
        else
        {//If we have enemies
            if(enemStats == null)
            {//Store the character stats of the enemy
                enemStats = CurrentAttackingEnemy.GetComponent<CharacterStats>();
            }
            else
            {    //and check it's health to see if he is dead
                if(enemStats.Health <= 0)
                { 
                    if(gm.CurrentPlayers.Contains(CurrentAttackingEnemy))
                    {
                        gm.CurrentPlayers.Remove(CurrentAttackingEnemy);
                    }

                    enemStats = null;                   
                    CurrentAttackingEnemy = null;
                }
            }
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

    void CheckCurrentEnemy()
    {
        if(CurrentAttackingEnemy)
        {
            if(enemStats)
            {
                if(enemStats.dead)
                {
                    CurrentAttackingEnemy = null;
                    enemStats = null;
                }
            }
        }
    }

    bool IsEnemyAttacking()
    {
        //Checks if the enemy is attacking by checking the variables of the Animator
        //We use the Animator here since it's a component we share through the characters 

        bool retVal = false;

        if(CurrentAttackingEnemy)
        {
            Animator enAnim = CurrentAttackingEnemy.GetComponent<Animator>();
             retVal = enAnim.GetBool("Attack");
        }

        return retVal;
    }

    float ikWeight;
    float curWeight;
    void OnAnimatorIK()
    {
        if (CurrentAttackingEnemy)
        {
            lookingPosition = CurrentAttackingEnemy.position;
            ikWeight = 1;
        }
        else
        {
            ikWeight = 0;
        }

        curWeight = Mathf.MoveTowards(curWeight, ikWeight, Time.deltaTime * 5);

        anim.SetLookAtWeight(curWeight, .5f, 1, 1, 1);
        anim.SetLookAtPosition(lookingPosition + new Vector3(0, 1.5f, 0));

		if (weapon != null && CurrentAttackingEnemy) {
			LeftHandIKWeight = Mathf.Lerp (LeftHandIKWeight, 1f, .045f * Time.deltaTime);
			anim.SetIKPosition (AvatarIKGoal.LeftHand, CurrentAttackingEnemy.position);
			anim.SetIKPositionWeight (AvatarIKGoal.LeftHand, LeftHandIKWeight);
		}
    }
}
