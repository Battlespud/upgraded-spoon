using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArmyList {


	public int[] ArmyUnits = new int[FactionsEnum.MaxIndex];

	bool initialized = true; //deprecated

	public void AddUnits(UnitType u, int number){
		if (!initialized) {
			for (int i = 0; i <= FactionsEnum.AllUnits.Count(); i++) {
				ArmyUnits [i] = 0;
				Debug.Log ("Army list initialized");
			}
			initialized = true;
		}
		int index = u.INDEX;
		ArmyUnits [index] += number;
		Debug.Log ("Army now contains " + ArmyUnits [index] + " " + FactionsEnum.AllUnitsNames[index]);
	}


}
