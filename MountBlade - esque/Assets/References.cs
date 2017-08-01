using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class References : MonoBehaviour {

	//just used to monitor the player character and keep a reference to it for ease of use

	public Character PlayerCharacter;

	public Races CharRace;
	public Factions CharFaction;

	public int[] playerArmy;

	//used by the UI buttons in the faction and race menu
	public void SetupPlayerRace(Races rac){
		PlayerCharacter.SetupRace (rac);
	}

	public void SetupPlayerFaction(Factions fac){
		PlayerCharacter.SetupFaction (fac);
	}

	// Use this for initialization
	void Awake () {
		PlayerCharacter = new Character();
		playerArmy = PlayerCharacter.armyList.ArmyUnits;
		PlayerCharacter.charID = 0;
		Character.CharacterDictionary.Add (PlayerCharacter.charID, PlayerCharacter);
	}
	
	// Update is called once per frame
	void Update () {
			CharFaction = PlayerCharacter.faction;
			CharRace = PlayerCharacter.race;
	}
}
