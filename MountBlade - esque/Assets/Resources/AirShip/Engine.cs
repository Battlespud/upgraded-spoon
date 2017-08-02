using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : ShipComponent {

	public float MaxPower = 20000; //20k
	public float FuelUse;
	public Rigidbody ShipRB;
	public ShipStats shipStats;

	[Range(0f,1f)]public float PowerLevel;

	// Use this for initialization
	void Start () {
		ShipRB = GetComponentInParent<Rigidbody> ();
		shipStats = GetComponentInParent<ShipStats> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		FuelUse = 1f * Time.fixedDeltaTime * (PowerLevel);
		if(shipStats.fuelTank.UseFuel(FuelUse))
		ShipRB.AddForce (transform.forward * MaxPower * PowerLevel);

	}
}
