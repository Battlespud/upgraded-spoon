using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyTankLeak : MonoBehaviour {

	//leaks a set amount of tank.Contents each second.  Not controllable by player. We'll add to this leak rate on hit later.

	BuoyancyTank tank;

	public float leakRate;

	// Use this for initialization
	void Start () {
		tank =	GetComponent<BuoyancyTank> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(tank.Contents > 0f)
		tank.Contents -= leakRate * Time.deltaTime;

		}
	}
