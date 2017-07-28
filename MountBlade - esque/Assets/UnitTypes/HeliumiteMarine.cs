using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliumiteMarine : UnitType {


	public override void Initialize ()
	{
		UnitTypeName = "Heliumite Marine";
		Ranged = true;
		Melee = true;
		cost = 100;
		race = Races.RED;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
