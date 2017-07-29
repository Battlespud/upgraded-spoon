using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZodanganWarrior : UnitType {


	public override void Initialize ()
	{
		INDEX = 21;
		UnitTypeName = "Zodangan Warrior";
		Ranged = true;
		Melee = true;
		cost = 90;
		UnitDescription = "Zodangan warriors are known both for their ferocity in the melee, and their brutality afterwards.";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
