using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipStats : MonoBehaviour {

	public List<BuoyancyTank> Tanks;
	public List<BuoyancyTankLeak> Leaks;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		Tanks = new List<BuoyancyTank> ();
		Leaks = new List<BuoyancyTankLeak> ();

		foreach (BuoyancyTank t in GetComponentsInChildren<BuoyancyTank>()) {
			Tanks.Add (t);
		}
		foreach (BuoyancyTank b in Tanks) {
			b.NumberOfTanks = Tanks.Count;
		}
		foreach (BuoyancyTankLeak t in GetComponentsInChildren<BuoyancyTankLeak>()) {
			Leaks.Add (t);
		}
	}
	
	// Update is called once per frame
	void Update () {
		TotalAntiGravForce = 0f;
		foreach (BuoyancyTank b in Tanks) {
			TotalAntiGravForce += b.antigravy;
		}
		GravForce = rb.mass * Physics.gravity.y;
		Vel = rb.velocity;
	}

	public float TotalAntiGravForce;
	public float GravForce;
	public Vector3 Vel;

	Rigidbody rb;
}
