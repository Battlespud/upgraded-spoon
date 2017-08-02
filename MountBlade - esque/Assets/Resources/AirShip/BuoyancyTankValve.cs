using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyTankValve : ShipComponent {

	//controllably release rays to alter buoyancy

	BuoyancyTank tank;

	public const float MaxVent = 2f;
	[Range(0f,MaxVent)]
	public float ventRate;

	public bool Automatic;

	// Use this for initialization
	void Start () {
		tank =	GetComponent<BuoyancyTank> ();
	}

	// Update is called once per frame
	void Update () {
		if(tank.Contents > 0f)
			tank.Contents -= ventRate * Time.deltaTime;

	}
}
