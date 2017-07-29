﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Factions{
	HELIUM,
	ZODANGA,
	FIRSTBORNCORSAIRS,
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
		"FirstBorn Corsairs",
		"FirstBorn of Omean",
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


	//races
	public static List<UnitType> RedUnitList;

	//final
	public static CombinedUnits HeliumCombined;
	public static CombinedUnits ZodangaCombined;


	static FactionsEnum(){

		//factions
		HeliumUnitList = new List<UnitType>(){HeliumiteMarine.CreateInstance<HeliumiteMarine>()};
		ZodangaUnitList = new List<UnitType> (){ ZodanganWarrior.CreateInstance<ZodanganWarrior>() };

		//races
		RedUnitList = new List<UnitType>(){RedInfantry.CreateInstance<RedInfantry>()};

		//final
		HeliumCombined = new CombinedUnits(RedUnitList, HeliumUnitList);
		ZodangaCombined = new CombinedUnits(RedUnitList, ZodangaUnitList);

		FactionUnitLists = new List<CombinedUnits> (){HeliumCombined,ZodangaCombined};

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