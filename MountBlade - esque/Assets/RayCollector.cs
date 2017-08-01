using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCollector : MonoBehaviour {

	//refills ray tanks

	public ShipStats shipStats;

	public bool Enabled = true;
	public const float MaxGenerationRate = .15f;
	[Range(0f,MaxGenerationRate)]public float GenerationRate;

	// Use this for initialization
	void Start () {
		shipStats = GetComponentInParent<ShipStats> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (enabled) {
			float rays = Time.deltaTime * GenerationRate;
			foreach (BuoyancyTank b in shipStats.RayTanks) {
				b.Contents += GenerationRate*Time.deltaTime / shipStats.RayTanks.Count;
			}
		}
	}
}
