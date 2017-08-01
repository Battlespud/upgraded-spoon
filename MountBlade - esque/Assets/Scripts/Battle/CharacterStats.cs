using UnityEngine;
using System.Collections;

public class CharacterStats : MonoBehaviour {

    public bool Enemy;
  //  public string characterID;//For dynamic tagging purposes
	public int FactionID; //owner faction
	public int CharacterID; //owner
    public float Health = 100;
    public bool immortal;
    public bool dead;
    public float AttackRange = 3;
    public bool blocking;
    //If the character is the one the player can control, it will help later on multiplater also
    public bool playerControlled = true;

    public bool riding;//If we are riding
    public bool canMount;//If we can mount a horse
    bool mounted; //if we have mounted a horse
    public Transform mount;//Our horse transform
    HorseControlPlayer hcp;//Our horses controller

    //Our references
    PlayerControl playerControl;
    CapsuleCollider capCol;
    Rigidbody rigidBody;
    Animator anim;
    FreeCameraLook cameraLook;
    GameManager gm;

    public GameObject damageCollider_Norm;
    public GameObject damageCollider_Riding;

    public Rigidbody[] ragdollRigid;
    public Collider[] ragdollColliders;









	//MIRRORING FOR AIR BATTLES
	public bool inMirrorMode;
	CharacterStatsProxy proxy;


	//DissolveEffect
	public NewDissolveScript dissolveEffect;

	// Use this for initialization
	void Start () 
    {
        if(GetComponent<PlayerControl>())
        {
            if(!GetComponent<PlayerControl>().isActiveAndEnabled)
            {
                Enemy = true;
            }
        }

        playerControl = GetComponent<PlayerControl>();
        capCol = GetComponent<CapsuleCollider>();
        rigidBody = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        //Only store the camera if it's a player we control
        if(playerControlled)
        {
            cameraLook = GameObject.FindGameObjectWithTag("CameraHolder").GetComponent<FreeCameraLook>();
        }

        CloseDamageColliders();
        CloseRagdoll();
	}

	void RemoveRB(){
		CloseRagdoll ();
		foreach (Collider col in ragdollColliders) {
			col.enabled = false;
		}
		capCol.enabled = false;
		Invoke ("DestroyGameObject", 1);
	}

	void DestroyGameObject(){
		Destroy (gameObject);
	}

	void Update () 
    {
        MountLogic();

        blocking = anim.GetBool("Block");

        if (immortal)
        {
            if (Health < 0)
                Health = 10;
        }
        else if (Health < 0)
        {
            if (!dead) //If we are dead
            {
                //we remove the transform from the game manager's list of players
                OpenRagdoll();
				gm.RemoveSoldier(transform);
				//Destroy(damageCollider_Norm);
				//Destroy (damageCollider_Riding);
                Health = 0;
                dead = true;
				Invoke ("RemoveRB", .5f);
                //anim.SetBool("Dead", true); //and we also inform the animator about it
            }
        }
			

        if(dead) //and we close all the components, this part could also go in the above statement so that we are not doing it every frame
        {
            if (GetComponent<PlayerControl>())
            {
                playerControl.enabled = false;
            }

            rigidBody.isKinematic = true;
            capCol.isTrigger = true;
            if (Enemy)
            { 
                GetComponent<EnemyControl>().enabled = false;
                GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
            }
			DissolveEffect ();

        }

		if (inMirrorMode) {
			ProxyLoop ();
		}
        
	}
    
	public void ProxyLoop(){
		
	}


	void DissolveEffect(){
		if (dissolveEffect != null)
		dissolveEffect.running = true;
	}

    void CloseRagdoll()
    {
        for (int i = 0; i < ragdollRigid.Length; i++)
        {
            ragdollRigid[i].isKinematic = true;
        }

        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = false;
            ragdollColliders[i].gameObject.layer = 2;
        }
    }

    void OpenRagdoll()
    {
        if(!ragdolled)
        {
            StartCoroutine("ragdollCoroutine");
            ragdolled = true;
        }
    }

    bool ragdolled;

    IEnumerator ragdollCoroutine()
    {
        for (int i = 0; i < ragdollColliders.Length; i++)
        {
            ragdollColliders[i].enabled = true;
        }
        yield return new WaitForEndOfFrame();

        for (int i = 0; i < ragdollRigid.Length; i++)
        {
            ragdollRigid[i].isKinematic = false;
        }

        yield return new WaitForEndOfFrame();
        anim.enabled = false;
    }


    //To be able to mount, we need to pass the mount's transform
    public void EnableMounting(Transform _mount)
    {
        this.mount = _mount;
        canMount = true;
    }

    //Disables the mounting options
    public void DisableMounting()
    {
        canMount = false;
        mount = null;
    }

    void MountLogic()
    {
        //If we are inside a trigger of a horse and we can mount and this character is a player character
        if(canMount && !mounted && playerControlled)
        {
            //Mount with F
            if(Input.GetKeyUp(KeyCode.F))
            {
                RideMount();
            }
        }
        else
        {
            if(mounted)
            {
                //Dismount with F
                 if(Input.GetKeyUp(KeyCode.F))
                 {
                     Dismount();
                 }
            }
        }
    }

    void RideMount()
    {
        //Here we basically disable the controller of the character and enable that of the horse
        mounted = true;
        hcp = mount.GetComponent<HorseControlPlayer>();
        hcp.playerControlled = true;
        riding = true;

        //Instead of clamping the values here you can lerp each one to get a more smooth transition
        //Note that this function is called only once so the lerp should be done on a different function
        transform.rotation = mount.rotation;
        transform.parent = hcp.riderPositionTransform; //the parent is the bone we've set on the horse controller
        transform.localPosition = Vector3.zero; //Change the position if it doesn't look correct
        capCol.enabled = false;
        rigidBody.isKinematic = true;
        cameraLook.target = mount;//The target of our camera is the mount now, we do that because we made the rigidbody of the character as kinematic

        //Decide which animation to player, not that this will stop any other animation you might be playing
        if (!hcp.riderAtLeftSide)
            anim.Play("Get_Up_On_Horse_R");
        else
            anim.Play("Get_Up_On_Horse_L");

        anim.SetBool("Riding", true);
    }

    void Dismount()
    {
        //Simply reset everything as it was
        mount = null;
        hcp.playerControlled = false;
        riding = false;
        mounted = false;
        transform.parent = null;
        capCol.enabled = true;
        rigidBody.isKinematic = false;
        cameraLook.target = transform;
        anim.SetBool("Riding", false);
        //Note that you should probably add a dismount animation over here

    }

    IEnumerator closeNoDamage()
    {
        yield return new WaitForSeconds(0.4f);
        noDamage = false;
    }

    public void OpenDamageCollider()
    {
        damageCollider_Norm.SetActive(true);
    }

    public void CloseDamageColliders()
    {
        damageCollider_Norm.SetActive(false);
        damageCollider_Riding.SetActive(false);
    }

    bool noDamage;

    public virtual void DoDamage(float amount)
    {
        if(!noDamage)
        {
            Health -= amount;
            StartCoroutine("closeNoDamage");
			Debug.Log (string.Format ("{0} taken by {1}. {2} health remains.", amount, name, Health));
            //TODO: Add hurting animation
            noDamage = true;
        }
    }
}
