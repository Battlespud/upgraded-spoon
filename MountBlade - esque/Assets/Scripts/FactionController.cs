using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FactionController : MonoBehaviour {
	//singletons
	//left as mono instead of SO for monitoring purposes.
	//Automatically placed ingame by FactionGenerator.

	public static List<FactionController> FactionList = new List<FactionController>();

	static string path = "Graphics/Factions/";

	public Sprite Flag;

	public Government gov;

	public string factionName;
	public Races factionRace;
	public List<UnitType> AvailableUnits;

	public int factionId;
	public Color factionColor;

	public int startingCharacters;
	public GameObject characterPrefab;

	public int currentArmyUnits;
	public int storePreviousArmyUnits;
	public int targetArmyUnits;

	public int factionWealth;
	public int factionWealthTarget;

	public List<Factions> EnemiesWith;
	public List<CampaignMap_AIUnit_Planet> FactionCharacters;
	public List<CampainMap_POI> FactionCastles;

	public List<Character> FactionLeaders;
	public List<CampainMap_POI> FactionVillages;

	public FactionAIstate factionAIstate;
	public FactionTraits factionTraits;
	public FactionPrefers factionPrefers;

	public enum FactionAIstate
	{
		GoToWar,RaiseArmies,RaiseResources
	}

	public enum FactionTraits
	{
		Aggressive,Passive,Diplomatic,Vengeful,Defensive
	}

	public enum FactionPrefers
	{
		Oranges,Apples,Lemons,Cake
	}





	public void SetupFaction(Factions fac){
		factionId = (int)fac;
		factionName = FactionsEnum.FactionNames [factionId];
		factionRace = FactionsEnum.FactionRaces [factionId];
		AvailableUnits = FactionsEnum.FactionUnitLists [factionId].Combined;
		Debug.Log ("FactionID: " + factionId + " Name: " + factionName);
		string shortName = factionName.Replace(" ",string.Empty);
		try{
			Flag = Resources.Load<Sprite> (string.Format ("{0}{1}/Flag",path,shortName)) as Sprite;
			if(Flag != null)
			Debug.Log(factionName +" has loaded its flag.");
		}
		catch{
		}
		switch (fac) {
		case (Factions.HELIUM):{
				factionColor = Color.blue;

				factionTraits = FactionTraits.Defensive;
				break;
			}

		case (Factions.ZODANGA):{
				factionColor = Color.red;

				factionTraits = FactionTraits.Aggressive;
				break;
			}
				
		case (Factions.CORSAIRS):{
				factionColor = Color.black;

				factionTraits = FactionTraits.Aggressive;
				break;
			}

		default:
			{

				break;
			}
		}
	}

















	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
