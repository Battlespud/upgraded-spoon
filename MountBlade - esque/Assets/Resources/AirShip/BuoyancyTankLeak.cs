﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyancyTankLeak : ShipComponent {

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