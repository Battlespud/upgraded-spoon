using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Origin
{
	BARSOOM,
	HYBORIA,
	EARTH
};

public enum Races
{
	RED,
	BLACK,
	WHITE,
	GREEN,
	HUMAN,
	HYBORIAN
	};

public enum Factions{
	HELIUM,
	ZODANGA,
	CORSAIRS,
	FIRSTBORN,
	HOLYTHERNS,
	WARHOON,
	THARK,
	JASOOM,
	WEST,
	SOUTH,
	SOUTHEAST,
	EAST,
	BARBARIANS
}


//This class is really sensitive and needs a safespace so pls no criticize.  It knows its shit. It accepts it.

public static class FactionsEnum {

	//dont make unit ids higher than this
	public const int MaxIndex = 200;

	public static List<string> FactionNames = new List<string> () {
		"Helium",
		"Zodanga",
		"Corsairs",  //firstborn corsairs
		"FirstBorn", //Firstborn of Omean
		"Holy Therns",
		"Warhoon",
		"Thark",
		"Earth", //earth
		"Apollonia",
		"Stygia",
		"Vendhya",
		"Khitai",
		"Barbarians"
	};

	public static List<string> FactionNamesLong = new List<string> () {
		"The Empire of Helium",
		"The Empire of Zodanga",
		"The Adherents of Issis",  //firstborn corsairs
		"The FirstBorn of Omean", //Firstborn of Omean
		"The Holy Therns",
		"The Warhoon Horde",
		"The Thark",
		"Triplanetary",
		"The Kingdom of Apollonia",
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
		Races.HYBORIAN,
		Races.HYBORIAN,
		Races.HYBORIAN,
		Races.HYBORIAN,
		Races.HYBORIAN
	};



	public static List<string> RaceNames = new List<string>(){"Red Martian", "FirstBorn", "White Thern", "Green Martian", "Human", "Hyborian"};


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
	//public static List<UnitType> AllUnits;
	public static Dictionary<int,UnitType> AllUnits;
	public static Dictionary<int, string> AllUnitsNames;

	//Two Words. Static. Constructor. 
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

		//AllUnits = new List<UnitType> ();
		AllUnits = new Dictionary<int, UnitType>();
		AllUnitsNames = new Dictionary<int, string>();


		foreach (CombinedUnits combined in FactionUnitLists) {
			foreach (UnitType u in combined.Combined) {
				if (!AllUnits.ContainsKey (u.INDEX)) {
					Debug.Log (u.UnitTypeName + " " + u.INDEX);
					AllUnits.Add (u.INDEX, u);
					Debug.Log (AllUnits [u.INDEX].UnitTypeName + " added to main reference list");
					AllUnitsNames.Add (u.INDEX, u.UnitTypeName);
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