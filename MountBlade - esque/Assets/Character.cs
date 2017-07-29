using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Any named character
public class Character {

	public Races race;
	public string raceName;

	public Factions faction;
	public string factionName;


	public void SetupFactionAndRace(Races rac, Factions fac){
		race = rac;
		raceName = Race.RaceNames [(int)race];

		faction = fac;
		factionName = FactionsEnum.FactionNames [(int)faction];
	}

	public void SetupFaction(Factions fac){
		faction = fac;
		factionName = FactionsEnum.FactionNames [(int)faction];
	}

	public void SetupRace(Races rac){
		race = rac;
		raceName = Race.RaceNames [(int)race];
	}

}
