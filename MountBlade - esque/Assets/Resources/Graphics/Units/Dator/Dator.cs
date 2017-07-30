using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dator : UnitType {

	public override void Initialize ()
	{
		INDEX = 42;
		UnitTypeName = "Dator";
		Ranged = false;
		Melee = true;
		cost = 225;
		UnitDescription = "The Black Dators of the FirstBorn comprise the warrior-nobility of their ancient and terrible race.";
	}

}
