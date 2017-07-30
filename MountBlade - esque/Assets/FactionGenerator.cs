using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactionGenerator : MonoBehaviour {

	FactionController Helium;
	FactionController Zodanga;
	FactionController Corsairs;


	// Use this for initialization
	void Awake () {
		Helium = gameObject.AddComponent<FactionController> (); Helium.SetupFaction (Factions.HELIUM); FactionController.FactionList.Add(Helium);
		Zodanga = gameObject.AddComponent<FactionController> (); Zodanga.SetupFaction (Factions.ZODANGA); FactionController.FactionList.Add(Zodanga);
		Corsairs = gameObject.AddComponent<FactionController> (); Corsairs.SetupFaction (Factions.CORSAIRS); FactionController.FactionList.Add(Corsairs);

		string allFactionsString = "Factions ";
		foreach (FactionController fac in FactionController.FactionList) {
			allFactionsString += " " + fac.factionName;   //TODO for some reason the first name is null, despite being successfully set before this
		}
		allFactionsString += " have been initialized!";

		Debug.Log (allFactionsString + " " + FactionController.FactionList.Count);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
