using UnityEngine;
using System.Collections;

public class WeaponCollisions : MonoBehaviour {

	//idk not mine.

    PlayerControl _PC;
    CharacterStats chS;
    EnemyControl enC;
    bool Enemy;
    bool Attacking;

	// Use this for initialization
	void Start () {

        chS = GetComponentInParent<CharacterStats>();

        Enemy = chS.Enemy;

        if (!Enemy)
            _PC = GetComponentInParent<PlayerControl>();
        else
            enC = GetComponentInParent<EnemyControl>();
	}
	
    void Update()
    {
        if (!Enemy)
            Attacking = _PC.attack;
        else
            Attacking = enC.attacking;
    }

	void OnTriggerEnter(Collider other)
    {
        if (Attacking)
        {
            //If the collider that entered the trigger is another weapon
            if (other.gameObject.tag == "Weapon")
            {
                Debug.Log("weapon");

                if (!Enemy)
                    _PC.blockedAttack = true; //that probably means the enemy blocked our attack
                else
                    enC.blockedAttack = true;
                //you can add a check here to make sure that the enemy was actually blocking when we hit his weapon
            }
            else if (other.transform.GetComponentInParent<CharacterStats>())//If we hit a character
            {
                CharacterStats cS = other.transform.GetComponentInParent<CharacterStats>();

				if (cS.FactionID != chS.FactionID) //and that character wasn't our own
                {
                    //Do damage etc.
                    cS.Health -= 30;
                }
            }
            else //If none of the above, we probably hit a tree
            {
                if (!Enemy)
                    _PC.blockedAttack = true;
                else
                    enC.blockedAttack = true;

            }
        }
    }
}
