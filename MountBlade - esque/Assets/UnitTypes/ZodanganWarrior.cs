using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodanganWarrior : UnitType {


	public override void Initialize ()
	{
		UnitTypeName = "Zodangan Warrior";
		Ranged = true;
		Melee = true;
		cost = 90;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
