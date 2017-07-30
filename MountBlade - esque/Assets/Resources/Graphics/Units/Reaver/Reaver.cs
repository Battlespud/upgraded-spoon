using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaver : UnitType {

	public override void Initialize ()
	{
		INDEX = 41;
		UnitTypeName = "Reaver";
		Ranged = false;
		Melee = true;
		cost = 125;
		UnitDescription = "The Reavers of the FirstBorn descend from great black fleets enmasse to slaughter and enslave their unfortunate victims.";
	}

}
