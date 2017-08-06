using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : ShipComponent {

	//1 unit per second at full power.
	//Only one per ship is supported, a ship can function without one but thrusters/engines wont work.

	public float Capacity;
	public float Contents;

	public bool UseFuel(float f){
		if (Contents > 0) {
			Contents -= f;
			return true;
		} else {
				Contents = 0;
			return false;
		}



	}

	// Use this for initialization
	void Start () {
		Contents = Capacity;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
