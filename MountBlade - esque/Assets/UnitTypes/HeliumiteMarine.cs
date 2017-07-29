using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeliumiteMarine : UnitType {


	public override void Initialize ()
	{
		INDEX = 11;
		UnitTypeName = "Heliumite Marine";
		Ranged = true;
		Melee = true;
		cost = 100;
		UnitDescription = "Heliumite Marines deploy from their great battleships to fight the enemys of Helium on foot during assaults";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
