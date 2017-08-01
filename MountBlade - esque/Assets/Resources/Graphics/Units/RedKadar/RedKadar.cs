using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedKadar : UnitType {

	public override void Initialize ()
	{
		INDEX = 2;
		UnitTypeName = "Red Kadar";
		Ranged = true;
		Melee = true;
		cost = 200;
		UnitDescription = "The Kadar are firearm equipped soldiers drawn from the ranks of the city guards. What they lack" +
			" defensively they make up for in firepower.";
	}

}
