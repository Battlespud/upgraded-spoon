using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FactionAndRaceDropdownSetup : MonoBehaviour {

	//Controller for the Faction and Race UI.

	Dropdown dropdown;

	public bool FactionSetup;

	Campaign_Map_Manager manager;
	References refer;

	// Use this for initialization
	void Start () {
		refer = GameObject.FindGameObjectWithTag ("CampaignMapManager").GetComponent<References>();
		dropdown = GetComponent<Dropdown> ();	
		dropdown.ClearOptions ();
		if (FactionSetup) {
			dropdown.AddOptions (FactionsEnum.FactionNames);
		} else {
			dropdown.AddOptions (FactionsEnum.RaceNames);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (FactionSetup) {
			refer.SetupPlayerFaction ((Factions)dropdown.value);
			} else {
			refer.SetupPlayerRace ((Races)dropdown.value);
		}
	}
}
