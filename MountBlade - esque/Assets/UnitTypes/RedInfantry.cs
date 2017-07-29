using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedInfantry : UnitType {


	public override void Initialize ()
	{
		UnitTypeName = "Red Infantry";
		Ranged = false;
		Melee = true;
		cost = 75;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
