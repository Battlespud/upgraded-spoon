using System.Collections;
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

	public static List<List<System.Type>> FactionUnitLists = new List<List<System.Type>> (){
		FinalHeliumList,
		FinalZodangaList
	};



	public static bool initialized = false;
	public static void SetupLists(){
		initialized = true;
		FinalHeliumList.AddRange (HeliumUnitList);
		FinalHeliumList.AddRange (RedUnitList);

	}



	//factions
	public static List<System.Type> HeliumUnitList = new List<System.Type>(){typeof(HeliumiteMarine)};
	public static List<System.Type> ZodangaUnitList = new List<System.Type>(){typeof(ZodanganWarrior)};


	//races
	public static List<System.Type> RedUnitList = new List<System.Type>(){typeof(RedInfantry)};

	//final
	public static List<System.Type> FinalHeliumList = new List<System.Type>();
	public static List<System.Type> FinalZodangaList = new List<System.Type>();

}
