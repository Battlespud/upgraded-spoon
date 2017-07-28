using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipStats : MonoBehaviour {

	public List<BuoyancyTank> Tanks;
	public List<BuoyancyTankLeak> Leaks;
	NavMeshAgent agent;


	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		Tanks = new List<BuoyancyTank> ();
		Leaks = new List<BuoyancyTankLeak> ();
		try{
		agent = GetComponent<NavMeshAgent> ();
		}
		catch{
		};
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

		Stability = (TotalAntiGravForce / GravForce*-1f) - .60f;
		if (Stability <= 0) {
			Doomed = true;
		}
		UIStability = Mathf.Lerp(0,100, Mathf.InverseLerp (0f, .4f, Stability));
		if (Doomed) Doom();
	}

	void Doom(){
		if (!AlreadyDoomed) {
			if (agent) {
				Destroy (agent);
			}
			foreach (BuoyancyTank b in Tanks) {
				BuoyancyTankLeak leak = b.gameObject.AddComponent<BuoyancyTankLeak> ();
				Leaks.Add (leak);
				leak.leakRate = 2f;
				AlreadyDoomed = true;
			}
		}
	}

	public bool Doomed = false;
	public bool AlreadyDoomed = false;

	public float TotalAntiGravForce;
	public float GravForce;
	public Vector3 Vel;

	Rigidbody rb;

	public float Stability = 1f;
	public float UIStability;
}
