using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyTankLeak : MonoBehaviour {

	BuoyancyTank tank;

	public float leakRate;

	// Use this for initialization
	void Start () {
		tank =	GetComponent<BuoyancyTank> ();
	}
	
	// Update is called once per frame
	void Update () {
		tank.Contents -= leakRate * Time.deltaTime;
		if (tank.Contents <= 0) {
			Destroy (this);
		}
	}
}
