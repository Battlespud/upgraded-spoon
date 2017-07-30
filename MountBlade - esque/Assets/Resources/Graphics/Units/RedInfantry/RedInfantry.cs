using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedInfantry : UnitType {


	public override void Initialize ()
	{
		INDEX = 01;
		UnitTypeName = "Red Infantry";
		Ranged = false;
		Melee = true;
		cost = 75;
		UnitDescription = "Red Martian Infantry fight in decimalized units" +
		" and are armed with Longswords and Shortswords.";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
