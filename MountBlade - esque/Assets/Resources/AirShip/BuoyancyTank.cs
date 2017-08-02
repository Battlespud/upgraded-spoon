using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyTank : ShipComponent {


	/*
	 * Recommended values: 
	 * 1000 mass
	 * 1.5 Drag
	 * 5 Angular Drag
	 * */

	//stores rays to allow hovering. Think of it as helium basically.

	public float MaxCapacity;
	public float Contents;
	public float fillPercentage;


	public float NumberOfTanks;

	public Rigidbody ShipRB;

	public float antigravity;

	public BuoyancyTankValve Valve;

	Material material;


	void Awake(){
		if (!GetComponent<BuoyancyTankValve> ()) {
			gameObject.AddComponent<BuoyancyTankValve> ();
		}
		if (!gameObject.GetComponent<BuoyancyTankLeak> ()) {
			gameObject.AddComponent<BuoyancyTankLeak> ();
		}
	}

	// Use this for initialization
	void Start () {
		ShipRB = GetComponentInParent<Rigidbody> ();
		ShipRB.isKinematic = false;
		if (MaxCapacity == 0f) {
			MaxCapacity = 100f;
		}
		Contents = MaxCapacity;
		fillPercentage = Contents / MaxCapacity;
		material = GetComponent<Renderer> ().material;

		Valve = GetComponent<BuoyancyTankValve> ();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Contents <= 0f) {
			Contents = 0f;
		}
		if (Contents >= MaxCapacity) {
			Contents = MaxCapacity;
		}
			fillPercentage = Contents / MaxCapacity;
	//		ShipRB.AddForceAtPosition(Physics.gravity*-1f*((fillPercentage/100f)*GRAVITY*ShipRB.mass),transform.position);
		material.color = Color.Lerp (Color.green, Color.red, 1f - fillPercentage);

	
		//Debug.Log (AntiGravity.y);
		//Debug.Log (Physics.gravity);

	}

	void FixedUpdate(){
		Vector3 AntiGravity = new Vector3 (0f, (Physics.gravity.y * -1f / NumberOfTanks)*ShipRB.mass *fillPercentage, 0f);
		antigravity = AntiGravity.y;
		ShipRB.AddForceAtPosition((AntiGravity),transform.position);

	}


}
