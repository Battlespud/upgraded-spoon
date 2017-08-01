using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public GameObject Player;
    public GameObject AIprefab;
    public Transform enemySpawnPoint;
	public Transform playerSpawnPoint;
    public int totalPlayers;
    public int totalEnemies;

    Campaign_Map_Manager cmManager;
	public Texture2D cursor;

	//characters in the fight, we'll figure out how to get this once theyre actually attached to the units
	public List<Character> Characters;
	public List<Character> AllyCharacters;
	public List<Character> EnemyCharacters;


    //A simple list where we store every soldier in the game
    public List<Transform> CurrentSoldiers = new List<Transform>();
    
    //All the characters that are allied with (and therefor can be controlled by) the player
    public List<Transform> PlayerAllies = new List<Transform>();
	public List<Transform> EnemySoldiers = new List<Transform>();


    void Start()
    {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Cursor.SetCursor (cursor, new Vector2 (16f, 16f), CursorMode.ForceSoftware);
        //Search to see if there is a campaign manager game object
        if (GameObject.FindGameObjectWithTag("CampaignMapManager"))
        {
            //and then store it
            cmManager = GameObject.FindGameObjectWithTag("CampaignMapManager").GetComponent<Campaign_Map_Manager>();

            //and change the number of the player's army
			foreach (Character c in AllyCharacters) {
				totalPlayers += c.armyList.ArmyUnits.Count();
			}
            totalPlayers = cmManager.PlayerArmy;

            totalEnemies = cmManager.currEnemies;
            //We do it this way so that we can reuse the same script for instances when we don't start the game from the map, think multiplayer
        }

        //We want to create the army of our player
        CreatePlayerArmy();
        //and the opposing army
        CreateEnemyArmy();


		//CreatePlayerArmyNew();
		//CreateEnemyArmyNew();
    }

	void CreateNewPlayerArmy()
	{
		//Since we don't want everybody to spawn on the same position
		int rows = 0;
		int columns = 0;

		for (int i = 0; i < totalPlayers; i++)
		{
			//we store the original position
			Vector3 pos = playerSpawnPoint.position;
			//and we add to the column and substract from the rows
			//the first time this runs, both of this have 0 value, so the character will spawn at the original position
			pos.x += columns;
			pos.z -= rows;

			//we instantiate the character

			foreach (Character c in AllyCharacters) {
				for(int index = 0; i < c.armyList.ArmyUnits.Length; i++){
					if (c.armyList.ArmyUnits [index] != 0) {
						for(int troopNumber = c.armyList.ArmyUnits[index]; c.armyList.ArmyUnits[index] > 0; c.armyList.ArmyUnits[index] -=1){
							GameObject aiPla = Instantiate(FactionsEnum.AllUnits[index].Prefab, pos, Player.transform.rotation) as GameObject;
							//Match faction and character IDs to the owning character so we can readd them to the appropriate list if they survive.
							aiPla.GetComponent<CharacterStats>().FactionID = (int)c.faction;
							aiPla.GetComponent<CharacterStats> ().CharacterID = c.charID;
							//Add them to the lists for the fight
							CurrentSoldiers.Add(aiPla.transform);
							PlayerAllies.Add(aiPla.transform);
							aiPla.name = "Ally " + FactionsEnum.AllUnitsNames[index]; //This only helps in the editor to differentiate between enemies and allies
						}
					}
				}
			}

			//if we don't have enough characters on the column
			if (columns < 5)
			{
				columns++;//then add more
			}
			else
			{//If not
				//Reset the columns
				columns = 0;
				//And add to the row
				rows++;
			}


		}
	}

	void CreateNewEnemyArmy()
	{
		//Almost identical to the above function with a minor changes in the original position and the initialization of the characters
		int rows = 0;
		int columns = 0;

		for (int i = 0; i < totalEnemies; i++)
		{
			Vector3 pos = enemySpawnPoint.position;
			pos.x += columns;
			pos.z -= rows;

			foreach (Character c in EnemyCharacters) {
				for(int index = 0; i < c.armyList.ArmyUnits.Length; i++){
					if (c.armyList.ArmyUnits [index] != 0) {
						for(int troopNumber = c.armyList.ArmyUnits[index]; c.armyList.ArmyUnits[index] > 0; c.armyList.ArmyUnits[index] -=1){
							GameObject aiPla = Instantiate(FactionsEnum.AllUnits[index].Prefab, pos, Player.transform.rotation) as GameObject;
							aiPla.GetComponent<CharacterStats>().FactionID = (int)c.faction;
							aiPla.GetComponent<CharacterStats> ().CharacterID = c.charID;						
							aiPla.name = "Enemy " + FactionsEnum.AllUnitsNames[index]; //This only helps in the editor to differentiate between enemies and allies
							CurrentSoldiers.Add(aiPla.transform);
							EnemySoldiers.Add(aiPla.transform);
						}
					}
				}
			}
			if (columns < 5)
			{
				columns++;
			}
			else
			{//If not
				//Reset the columns
				columns = 0;
				//And add to the row
				rows++;
			}

		}
	}

    void CreatePlayerArmy()
    {
        //Since we don't want everybody to spawn on the same position
        int rows = 0;
        int columns = 0;

        for (int i = 0; i < totalPlayers; i++)
        {
            //we store the original position
			Vector3 pos = playerSpawnPoint.position;
            //and we add to the column and substract from the rows
            //the first time this runs, both of this have 0 value, so the character will spawn at the original position
            pos.x += columns;
            pos.z -= rows;

            //we instantiate the character
            GameObject aiPla = Instantiate(AIprefab, pos, Player.transform.rotation) as GameObject;

            //if we don't have enough characters on the column
            if (columns < 5)
            {
                columns++;//then add more
            }
            else
            {//If not
                //Reset the columns
                columns = 0;
                //And add to the row
                rows++;
            }

            //Since these are Allies, make them the same ID as the player
            aiPla.GetComponent<CharacterStats>().CharacterID = Player.GetComponent<CharacterStats>().CharacterID;
            //Add them to the lists
			CurrentSoldiers.Add(aiPla.transform);
            PlayerAllies.Add(aiPla.transform);
            aiPla.name = "Ally"; //This only helps in the editor to differentiate between enemies and allies
        }
    }

    void CreateEnemyArmy()
    {
        //Almost identical to the above function with a minor changes in the original position and the initialization of the characters
        int rows = 0;
        int columns = 0;

        for (int i = 0; i < totalEnemies; i++)
        {
            Vector3 pos = enemySpawnPoint.position;
            pos.x += columns;
            pos.z -= rows;

            GameObject aiPla = Instantiate(AIprefab, pos, enemySpawnPoint.rotation) as GameObject;

            if (columns < 5)
            {
                columns++;
            }
            else
            {//If not
                //Reset the columns
                columns = 0;
                //And add to the row
                rows++;
            }

			aiPla.GetComponent<CharacterStats>().name = "Enemy";
			CurrentSoldiers.Add(aiPla.transform);
            aiPla.name = "Enemy";
        }
    }

    //A simple function so that we don't have to refer to the lists from every script
    public void RemoveSoldier(Transform ch)
    {
		if (CurrentSoldiers.Contains(ch))
			CurrentSoldiers.Remove(ch);
    }
}
