using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Factions{
	HELIUM,
	ZODANGA,
	CORSAIRS,
	FIRSTBORN,
	HOLYTHERNS,
	WARHOON,
	THARK,
	WEST,
	SOUTH,
	SOUTHEAST,
	EAST,
	BARBARIANS
}


public static class FactionsEnum {


	public static List<string> FactionNames = new List<string> () {
		"Helium",
		"Zodanga",
		"Corsairs",  //firstborn corsairs
		"FirstBorn", //Firstborn of Omean
		"Holy Therns",
		"Warhoon",
		"Thark",
		"Apollonia",
		"Stygia",
		"Vendhya",
		"Khitai",
		"Barbarians"
	};

	public static List<Races> FactionRaces = new List<Races> () {
		Races.RED,
		Races.RED,
		Races.BLACK,
		Races.BLACK,
		Races.WHITE,
		Races.GREEN,
		Races.GREEN,
		Races.HUMAN,
		Races.HUMAN,
		Races.HUMAN,
		Races.HUMAN,
		Races.HUMAN
	};

	public static List<CombinedUnits> FactionUnitLists;

	//factions
	public static List<UnitType> HeliumUnitList;
	public static List<UnitType> ZodangaUnitList;

	public static List<UnitType> CorsairsUnitList;
	public static List<UnitType> FirstBornUnitList;

	//races
	public static List<UnitType> RedUnitList;
	public static List<UnitType> BlackUnitList;

	//final
	public static CombinedUnits HeliumCombined;
	public static CombinedUnits ZodangaCombined;

	public static CombinedUnits CorsairsCombined;
	public static CombinedUnits FirstBornCombined;

	public static List<CombinedUnits> ListOfCombined;

	//All
	//hopefully contains one of each unit... hopefully
	public static List<UnitType> AllUnits;
	public static List<string> AllUnitsNames;

	static FactionsEnum(){
		//factions
		HeliumUnitList = new List<UnitType>(){HeliumiteMarine.CreateInstance<HeliumiteMarine>()};
		ZodangaUnitList = new List<UnitType> (){ ZodanganWarrior.CreateInstance<ZodanganWarrior>() };

		CorsairsUnitList = new List<UnitType> (){ Reaver.CreateInstance<Reaver> () };
		FirstBornUnitList = new List<UnitType> (){};


		//races
		RedUnitList = new List<UnitType>(){RedInfantry.CreateInstance<RedInfantry>()};
		BlackUnitList = new List<UnitType>(){ Dator.CreateInstance<Dator> () };

		//final
		HeliumCombined = new CombinedUnits(RedUnitList, HeliumUnitList);
		ZodangaCombined = new CombinedUnits(RedUnitList, ZodangaUnitList);

		CorsairsCombined = new CombinedUnits (BlackUnitList, CorsairsUnitList);
		FirstBornCombined = new CombinedUnits (BlackUnitList, FirstBornUnitList);


		FactionUnitLists = new List<CombinedUnits> (){HeliumCombined,ZodangaCombined,CorsairsCombined,FirstBornCombined};

		//ListOfCombined = new List<CombinedUnits> (){ HeliumCombined, ZodangaCombined,CorsairsCombined,FirstBornCombined };

		AllUnits = new List<UnitType> ();
		AllUnitsNames = new List<string> ();


		foreach (CombinedUnits combined in FactionUnitLists) {
			bool cont = true;
			foreach (UnitType u in combined.Combined) {
			/*	foreach (UnitType inList in AllUnits) {
					if(inList.GetType() == u.GetType()){
						cont = false;
					}
				} */
				if (cont) {
					Debug.Log (u.UnitTypeName + " " + u.INDEX);
				//	AllUnits[u.INDEX] = u;
				//	AllUnitsNames[u.INDEX] = u.UnitTypeName;

				}
			}
		}

	}

}


public struct CombinedUnits{
	public List<UnitType> RaceUnits;
	public List<UnitType> FactionUnits;
	public List<UnitType> Combined;
	public List<string> CombinedNames;

	public CombinedUnits(List<UnitType> race, List<UnitType> fac){

		RaceUnits = race;
		FactionUnits = fac;

		foreach (UnitType t in RaceUnits) {
			t.Start ();
			if (t.UnitCard != null)
				Debug.Log (t.UnitTypeName + " has loaded its unitcard!");
		}

		foreach (UnitType t in FactionUnits) {
			t.Start ();
			if (t.UnitCard != null)
				Debug.Log (t.UnitTypeName + " has loaded its unitcard!");
		}

		Combined = new List<UnitType> ();
		Combined.AddRange (RaceUnits);
		Combined.AddRange (FactionUnits);

		CombinedNames = new List<string> ();

		foreach (UnitType t in Combined) {
			CombinedNames.Add (t.UnitTypeName);
		}

	}
}