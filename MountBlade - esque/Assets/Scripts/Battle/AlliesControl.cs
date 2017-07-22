using UnityEngine;
using System.Collections;

public class AlliesControl : MonoBehaviour {

    GameManager gm;
    CharacterStats charStats;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        charStats = GetComponent<CharacterStats>();
    }

	void Update () 
    {
        //Based on which key we press, we change the states for every character in our allies list
        //If you want to separate between Archers,Melee,Cavalry then it's a simple case of accessing the correct list
        //and doing changes to that list only

        if(Input.GetKey(KeyCode.Alpha1))
             HoldPosition();

         if(Input.GetKey(KeyCode.Alpha2))
             Charge();

         if (Input.GetKey(KeyCode.Alpha3))
             Follow();
	}

    void HoldPosition()
    {
       foreach(Transform trans in gm.PlayerAllies)
        {
            trans.GetComponent<EnemyControl>().holdPosition = transform.position;
            trans.GetComponent<EnemyControl>().aiState = EnemyControl.AIState.Hold;
        }
    }

    void Charge()
    {
        foreach (Transform trans in gm.PlayerAllies)
        {
             trans.GetComponent<EnemyControl>().aiState = EnemyControl.AIState.Charge;
        }
    }

    void Follow()
    {
        foreach (Transform trans in gm.PlayerAllies)
        {
            trans.GetComponent<EnemyControl>().generalCharacter = transform;
            trans.GetComponent<EnemyControl>().aiState = EnemyControl.AIState.Follow;
        }
    }
}
