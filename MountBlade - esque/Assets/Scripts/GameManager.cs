using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour {

    public GameObject Player;
    public GameObject AIprefab;
    public Transform enemySpawnPoint;
    public int totalPlayers;
    public int totalEnemies;

    Campaign_Map_Manager cmManager;
	public Texture2D cursor;


    //A simple list where we store every character in the game
    public List<Transform> CurrentPlayers = new List<Transform>();
    
    //All the characters that are allied with (and therefor can be controlled by) the player
    public List<Transform> PlayerAllies = new List<Transform>();

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
            totalPlayers = cmManager.PlayerArmy;

            totalEnemies = cmManager.currEnemies;
            //We do it this way so that we can reuse the same script for instances when we don't start the game from the map, think multiplayer
        }

        //We want to create the army of our player
        CreatePlayerArmy();
        //and the opposing army
        CreateEnemyArmy();
    }

    void CreatePlayerArmy()
    {
        //Since we don't want everybody to spawn on the same position
        int rows = 0;
        int columns = 0;

        for (int i = 0; i < totalPlayers; i++)
        {
            //we store the original position
            Vector3 pos = Player.transform.position;
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
            aiPla.GetComponent<CharacterStats>().characterID = Player.GetComponent<CharacterStats>().characterID;
            //Add them to the lists
            CurrentPlayers.Add(aiPla.transform);
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

            aiPla.GetComponent<CharacterStats>().characterID = "Enemy";
            CurrentPlayers.Add(aiPla.transform);
            aiPla.name = "Enemy";
        }
    }

    //A simple function so that we don't have to refer to the lists from every script
    public void RemoveCharacter(Transform ch)
    {
        if (CurrentPlayers.Contains(ch))
            CurrentPlayers.Remove(ch);
    }
}
