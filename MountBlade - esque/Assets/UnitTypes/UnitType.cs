using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitType : ScriptableObject {


	public  string UnitTypeName;

	public int INDEX;

	public bool Melee;
	public bool Ranged;

	public bool Skirmisher;

	public int cost;

	public string UnitDescription;


	//Graphics
	public static string path = "Graphics/Units/";

	public Sprite UnitCard;
	public GameObject Prefab;

	//blahblah health, weapons etc



	public void LoadGraphics(){
		try{
			string shortName = UnitTypeName.Replace(" ",string.Empty);
			UnitCard = Resources.Load<Sprite> (string.Format ("{0}{1}/UnitCard", path, shortName )) as Sprite;
		}
		catch{
		} //idgaf
	}

	// Use this for initialization
	public void  Start () {
		Initialize ();
		LoadGraphics ();
		if (Melee && Ranged){Skirmisher = true;	}


	}

	public abstract void Initialize ();

	// Update is called once per frame
	void Update () {
		
	}
}
