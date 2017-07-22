using UnityEngine;
using System.Collections;

public class DamageColliders : MonoBehaviour {

    public int attackType;
    PlayerControl plControl;
    EnemyControl enControl;
    CharacterStats charStats;

	void Start () 
    {
        charStats = GetComponentInParent<CharacterStats>();
        plControl = GetComponentInParent<PlayerControl>();
        enControl = GetComponentInParent<EnemyControl>();
	}

    void OnTriggerEnter(Collider other)
    {      
        if(other.GetComponent<CharacterStats>())
        {
            CharacterStats enStats = other.GetComponent<CharacterStats>();

            if(enStats != charStats)
            {

                //TODO: Add more logic here before actually applying the damage
                //Examples: 
                /* Is the character facing us?
                 * Is it a friendly unit?
                 * Is the blocking corresponds to our attack?
                 */

                if (!enStats.blocking)
                {
                    enStats.DoDamage(30);
                }
                else
                {
                    if(plControl)
                    {
                        plControl.blockedAttack = true;
                    }

                    if(enControl)
                    {
                        enControl.blockedAttack = true;
                    }
                }
            }
        }
    }
}
