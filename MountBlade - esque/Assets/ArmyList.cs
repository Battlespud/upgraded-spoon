using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmyList {


	List<int> ArmyUnits = new List<int> ();

	bool initialized = true; //deprecated

	public void AddUnits(UnitType u, int number){
		if (!initialized) {
			for (int i = 0; i <= FactionsEnum.AllUnits.Count; i++) {
				ArmyUnits [i] = 0;
				Debug.Log ("Army list initialized");
			}
			initialized = true;
		}
		int index = u.INDEX;
		Debug.Log (index);
		ArmyUnits [index] += number;
		Debug.Log ("Army now contains " + ArmyUnits [index] + " " + FactionsEnum.AllUnitsNames[index]);
	}


}
