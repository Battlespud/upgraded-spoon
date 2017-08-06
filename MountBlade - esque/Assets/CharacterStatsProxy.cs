using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsProxy : MonoBehaviour {

	//Used for getting around navmesh limitations. Deprecated.  Use EnemyControlProxy instead.

	public CharacterStats RealCharacter;

	CharacterController Controller;

	Weapon weapon;

	public void SetRealCharacter(CharacterStats cha){
		RealCharacter = cha;
	}



	// Use this for initialization
	void  Start () {
	}
	
	// Update is called once per frame
	void  Update () {
		
	}

	public  void DoDamage(float amount){
		RealCharacter.DoDamage (amount);
		if(RealCharacter.dead){
			Destroy (gameObject);
			//TODO
		}
	}



}
