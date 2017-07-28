using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyTank : MonoBehaviour {

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
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Contents <= 0f) {
			Contents = 0f;
		}
			fillPercentage = Contents / MaxCapacity;
	//		ShipRB.AddForceAtPosition(Physics.gravity*-1f*((fillPercentage/100f)*GRAVITY*ShipRB.mass),transform.position);
		material.color = Color.Lerp (Color.green, Color.red, 1f - fillPercentage);

	
		//Debug.Log (AntiGravity.y);
		//Debug.Log (Physics.gravity);

	}

	void FixedUpdate(){
		Vector3 AntiGravity = new Vector3 (0f, (Physics.gravity.y * -1f / NumberOfTanks)*ShipRB.mass *fillPercentage, 0f);
		antigravy = AntiGravity.y;
		ShipRB.AddForceAtPosition((AntiGravity),transform.position);

	}

	public float MaxCapacity;
	public float Contents;
	public float fillPercentage;


	public float NumberOfTanks;

	public Rigidbody ShipRB;

	public float antigravy;


	Material material;
}
