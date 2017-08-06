using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ShipStats : MonoBehaviour {

	//Contains collections for all ship components and override controls.
	[SerializeField]public string Instructions = "This is the Ship's main control panel, use the sliders to control it. \n See individual components for better instructions.";
	[Header("Collections")]
	public List<BuoyancyTank> RayTanks;
	public FuelTank fuelTank;
	public List<BuoyancyTankLeak> Leaks;
	public List<Engine> Engines;
	public List<BuoyancyTankValve> Valves;
	public List<RayCollector> Collectors;
	public List<StabilizationThrusters> Thrusters; // used for stopping
	NavMeshAgent agent;

	float fuelUse;
	[Header("Fuel Information")]
	public float Fuel;
	public float FuelCapacity;
	public float SecondsOfFuel;

	[Header("Control Functionality")]
	//go forward
	[SerializeField]bool EngineOverride = true;
	[SerializeField][Range(0.0f,1.0f)] float EnginePowerLevelOverride;

	//release rays to lower antigravity force
	[SerializeField]bool ValveOverride = true;
	[SerializeField][Range(0.0f,BuoyancyTankValve.MaxVent)] float ValveVentOverride;

	//refill tanks
	[SerializeField]bool CollectorOverride = true;
	[SerializeField][Range(0.0f,RayCollector.MaxGenerationRate)] float CollectorRateOverride;
	[SerializeField]bool CollectorEnabledOverride = true;
	//stop or move up
	[SerializeField]bool StabilizerOverride = true;
	[SerializeField][Range(0.0f,1f)] float StabilizerPowerOverride;
	public  StabilizationThrusters.Mode ThrusterMode; 

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();

		RayTanks = new List<BuoyancyTank> ();
		Leaks = new List<BuoyancyTankLeak> ();
		try{
		agent = GetComponent<NavMeshAgent> ();
		}
		catch{
		};
		foreach (Engine e in GetComponentsInChildren<Engine>()) {
			Engines.Add (e);
		}
		foreach (BuoyancyTank t in GetComponentsInChildren<BuoyancyTank>()) {
			RayTanks.Add (t);
		}
		foreach (BuoyancyTank b in RayTanks) {
			b.NumberOfTanks = RayTanks.Count;
		}
		foreach (BuoyancyTankLeak t in GetComponentsInChildren<BuoyancyTankLeak>()) {
			Leaks.Add (t);
		}
		foreach (BuoyancyTankValve v in GetComponentsInChildren<BuoyancyTankValve>()) {
			Valves.Add (v);
		}
		foreach (RayCollector c in GetComponentsInChildren<RayCollector>()) {
			Collectors.Add (c);
		}
		foreach (StabilizationThrusters t in GetComponentsInChildren<StabilizationThrusters>()) {
			Thrusters.Add (t);
		}
		fuelTank = GetComponentInChildren<FuelTank>();
	}
	
	// Update is called once per frame
	void Update () {
		if (fuelTank) {
			FuelCapacity = fuelTank.Capacity;
			Fuel = fuelTank.Contents;
			fuelUse = 0f;
		}
		//The sum antigravity forces of all of our tanks.
		TotalAntiGravForce = 0f;
		foreach (BuoyancyTank b in RayTanks) {
			TotalAntiGravForce += b.antigravity;
		}
		//the force of gravity acting on us
		GravForce = rb.mass * Physics.gravity.y;
		Vel = rb.velocity;

		GravityRatio = TotalAntiGravForce / GravForce *-1f;

		//when we lose 40% antigrav capacity the ship is doomed and stress fractures cause leaks in all tanks.
		Stability = (TotalAntiGravForce / GravForce*-1f) - .60f;
		if (Stability <= 0) {
			Doomed = true;
		}
		//Map stability to a more useful range.
		UIStability = Mathf.Lerp(0,100, Mathf.InverseLerp (0f, .4f, Stability));
		if (Doomed) Doom();

		//pass instructions along to components
		if (EngineOverride) {
			foreach (Engine e in Engines) {
				e.PowerLevel = EnginePowerLevelOverride;
				fuelUse += e.FuelUse;
			}
		}
		if (ValveOverride) {
			foreach (BuoyancyTankValve v in Valves) {
				v.ventRate = ValveVentOverride;
			}
		}
		if (StabilizerOverride) {
			foreach (StabilizationThrusters v in Thrusters) {
				v.mode = ThrusterMode;
				v.power = StabilizerPowerOverride;
				fuelUse += v.FuelUse;
			}
		}
		if (CollectorOverride) {
			foreach (RayCollector c in Collectors) {
				c.GenerationRate = CollectorRateOverride;
				c.Enabled = CollectorEnabledOverride;
			}
		}

		if(fuelTank)
		SecondsOfFuel = Fuel / (fuelUse * 60f);
	}

	void FixedUpdate(){
		//physics heresy inc
		transform.RotateAround(transform.position,transform.up,TurnRate*TurnControl*Time.fixedDeltaTime);
	}



	void Doom(){
		if (!AlreadyDoomed) {
			if (agent) {
				Destroy (agent);
			}
			foreach (BuoyancyTank b in RayTanks) {
				BuoyancyTankLeak leak = b.gameObject.AddComponent<BuoyancyTankLeak> ();
				Leaks.Add (leak);
				leak.leakRate = 2f;
				AlreadyDoomed = true;
			}
		}
	}

	bool Doomed = false;
	bool AlreadyDoomed = false;

	public float GravityRatio;

	public float TotalAntiGravForce;
	public float GravForce;
	public Vector3 Vel;

	public float TurnRate = 15f;
	[Range(-1f,1f)]
	public float TurnControl = 0f;

	Rigidbody rb;

	public float Stability = 1f;
	public float UIStability;
}
