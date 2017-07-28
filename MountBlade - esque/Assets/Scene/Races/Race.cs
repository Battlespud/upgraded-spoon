using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	HUMAN


};


public class Race: MonoBehaviour {


	public static List<string> RaceNames = new List<string>(){"Red Martian", "FirstBorn", "White Thern", "Green Martian", "Human"};


	// Use this for initialization
	void Start () {
		
	}





	public string OriginName;
	public string RaceName;
	public List<UnitType> RaceUnitList;
	

}
