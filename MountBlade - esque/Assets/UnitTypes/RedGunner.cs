using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGunner : UnitType {

	public override void Initialize(){
		UnitTypeName = "Red Arquebusier";
		Ranged = true;
		Melee = false;

		cost = 175;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
