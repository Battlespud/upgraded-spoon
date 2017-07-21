using UnityEngine;
using System.Collections;

public class HorseTrigger : MonoBehaviour {

    //If the trigger is at the left side of the horse, we control the animations this way
    public bool leftSide;

    HorseControlPlayer hcp;
    CharacterStats cc;

	void Start () 
    {
        hcp = GetComponentInParent<HorseControlPlayer>();
	}
	
    
    void OnTriggerEnter(Collider other)
    {      
        if (!hcp.playerControlled)//If the horse doesn't have a rider
        {
            if (other.GetComponent<CharacterStats>()) //and a character enters
            {              
                cc = other.GetComponent<CharacterStats>();

                if (cc.playerControlled) //and that character is controlled by the Player
                {
                    cc.EnableMounting(hcp.transform); //Then he can mount the horse
                    
                    if(leftSide)
                    {
                        hcp.riderAtLeftSide = true;
                    }
                    else
                    {
                        hcp.riderAtLeftSide = false;
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<CharacterStats>())//if the collider was a character
        {
            if(cc != null)//and we have previously stored another character
            if(cc == other.GetComponent<CharacterStats>())//and it's the same like the one that exited
            {
                cc.DisableMounting();//then he can no longer mount the horse
                cc = null;
            }
        }
    }
}
