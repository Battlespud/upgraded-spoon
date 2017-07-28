using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour {

	public Character PlayerCharacter;

	public Races CharRace;
	public Factions CharFaction;

	public void SetupPlayerRace(Races rac){
		PlayerCharacter.SetupRace (rac);
	}

	public void SetupPlayerFaction(Factions fac){
		PlayerCharacter.SetupFaction (fac);
	}

	// Use this for initialization
	void Start () {
		PlayerCharacter = new Character();
	}
	
	// Update is called once per frame
	void Update () {
		if (PlayerCharacter.faction != null) {
			CharFaction = PlayerCharacter.faction;
			CharRace = PlayerCharacter.race;
		}
	}
}
