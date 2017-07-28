using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitType : MonoBehaviour {


	public string UnitTypeName;

	public Factions faction;
	public Races race;

	public bool Melee;
	public bool Ranged;

	public bool Skirmisher;

	public int cost;


	//blahblah health, weapons etc


	// Use this for initialization
	void Start () {
		Initialize ();
		if (Melee && Ranged){		Skirmisher = true;	}


	}

	public abstract void Initialize ();

	// Update is called once per frame
	void Update () {
		
	}
}
