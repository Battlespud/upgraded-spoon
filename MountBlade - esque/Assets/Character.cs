using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RelationType{
	DEFAULT,
	PARENTCHILD,
	SIBLINGSIBLING,
	GRANDPARENTGRANDCHILD,
	UNCLENEPHEW,
	RIVALRIVAL,
	FRIENDFRIEND

};

public struct Relation{
	RelationType relationship;
	Character char1;
	Character char2;
	//strength of the relationship. 0 is neutral. 
	float affection;

	//returns the other character in the relationship
	public Character GetOther(Character original){
		if (original != char1) {
			return char1;
		}
		return char2;
	}

	public Relation(RelationType r, Character a, Character b){
		relationship=r;
		char1 = a;
		char2 = b;
		affection = 0f;

		char1.Relationships.Add (this);
		char2.Relationships.Add (this);
	}
}

//Any named character
public class Character : ScriptableObject {



	//equivalent to a lord in Warband
	static int ID = 1; //add 1 whenever we assign, 0 is player
	public static Dictionary<int,Character> CharacterDictionary = new Dictionary<int, Character>(); //hopefully contains every character

	public static Character GetCharacter(int i){
		if (!CharacterDictionary.ContainsKey (i))
			Debug.Log ("Invalid Character ID: " + i);
		return CharacterDictionary [i];
	}

	//Do not add directly to Relationships list. Just instantiate a new relationship and it will automatically put itself where it needs to go
	public List<Relation> Relationships = new List<Relation> ();


	public Races race;
	public string raceName;

	public string title;
	public string firstName;
	public string lastName;
	public string GetName(){
		return string.Format ("{0} {1} {2} of {3}",title,firstName,lastName,FactionsEnum.FactionNamesLong[(int)faction]);
	}
	bool male = true; //There are only two types of people, those with dicks, and sandwich makers

	public int charID;

	public float loyalty;



	public Factions faction;
	public string factionName;

	public ArmyList armyList = new ArmyList();

	public void SetupFactionAndRace(Races rac, Factions fac){
		race = rac;
		raceName = FactionsEnum.RaceNames [(int)race];

		faction = fac;
		factionName = FactionsEnum.FactionNames [(int)faction];
	}

	public void SetupFaction(Factions fac){
		faction = fac;
		factionName = FactionsEnum.FactionNames [(int)faction];
	}

	public void SetupRace(Races rac){
		
		race = rac;
		raceName = FactionsEnum.RaceNames [(int)race];
	}

	public void Construct(string t, string f, string l, Races rac, Factions fac){
		title = t;
		firstName = f;
		lastName = l;
		SetupFactionAndRace (rac,fac);

		charID = ID;
		ID++;

		if (!CharacterDictionary.ContainsKey (charID)) {
			CharacterDictionary.Add (charID, this);
		}
	}

	//sets up  arelationship network based on reference relationship
	public void Construct(string t, string f, string l, Races rac, Factions fac, Character reference, Relation relWithReference){
		title = t;
		firstName = f;
		lastName = l;
		SetupFactionAndRace (rac,fac);

		charID = ID;
		ID++;

		if (!CharacterDictionary.ContainsKey (charID)) {
			CharacterDictionary.Add (charID, this);
		}
	}

}
