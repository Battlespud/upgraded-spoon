using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyTankValve : ShipComponent {

	//Same as leak, but user controllable.  Think of it as a ballast.


	BuoyancyTank tank;

	public const float MaxVent = 2f;
	[Range(0f,MaxVent)]
	public float ventRate;

	void Start () {
		tank =	GetComponent<BuoyancyTank> ();
	}

	void Update () {
		if(tank.Contents > 0f)
			tank.Contents -= ventRate * Time.deltaTime;

	}
}
