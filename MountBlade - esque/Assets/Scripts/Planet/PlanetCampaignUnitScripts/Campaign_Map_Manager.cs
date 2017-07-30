using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Campaign_Map_Manager : MonoBehaviour {

    /*This script is the main manager for our game
     * It will hold information about our players, enemies, factions etc.
     * Having everything in this script means that it would also be easier to save them ;)
     */

    public int PlayerArmy; //the number of units the player has
    public int currEnemies; //the number of enemies the POI we are going to has, this is used to instantiate them on the level we are loading
    public bool onMenu;//If the player is on a menu
    public bool tick;
    //We want a list with all the basic properties of our Factions, see below for more

    //Do note, for my projects, I usually keep everything that has to do with the UI on a different script to avoid confusions

	public string PlayerName;


	//Events for uiManager;
	StringEvent ChangeSceneEvent;

	UIManager uiManager;


    void Awake()
    {
		uiManager = GetComponent<UIManager> ();
		PreventDuplicationAndDontDestroy ();

		ChangeSceneEvent = new StringEvent ();
		ChangeSceneEvent.AddListener (uiManager.RecieveChangeSceneEvent);
    }

	void PreventDuplicationAndDontDestroy(){
		//We never want this object to be destroyed since it holds all our informations
		DontDestroyOnLoad(transform.gameObject);

		//However, if we are loading a scene that already has a gameobject like this, then we want to destroy that
		if(GameObject.FindGameObjectWithTag("CampaignMapManager"))
		{
			if(GameObject.FindGameObjectWithTag("CampaignMapManager") != this.gameObject)
			{
				Destroy(GameObject.FindGameObjectWithTag("CampaignMapManager"));
			}
		}
		//If the above doesn't work, use a loop to get all references and destroy the one you don't want
	}

    void Start()
    {
		PlayerName = "Player";
        //Let's close the UI element at start
    }

	public void NewGame(){
		InitializeSpace ();
	}

	void InitializeSpace (){
		ChangeSceneEvent.Invoke ("Space");
		SceneManager.LoadScene ("Space");
	}

	void InitializePlanet (){
		ChangeSceneEvent.Invoke ("Planet");
		SceneManager.LoadScene ("Planet");
	}

	void InitializeCity (){
		ChangeSceneEvent.Invoke ("City");

	}

	void InitializeBattle (){
		ChangeSceneEvent.Invoke ("VillageBattle");
		SceneManager.LoadScene ("VillageBattle");
	}




	//TODO Called by player upon arrival at a destination
	public void CheckDestination(CampainMap_POI.POItype destinationType)
	{
		//The type of our destination
		switch(destinationType)
		{
		case CampainMap_POI.POItype.Player: //Lines that do not have a break; between them means that they are the same case
		case CampainMap_POI.POItype.Terrain:
		default: 
			//Do nothing
			break;
		case CampainMap_POI.POItype.Castle:
			//Load Level
			InitializeBattle();
			break;
		case CampainMap_POI.POItype.Unit:
			//Load battlefield
			break;
		case CampainMap_POI.POItype.Village:
			//Load village UI
			onMenu = true;
			uiManager.EnableVillageUI ();
			break;
		}
	}



    //A simple function (for now) that increases the player's army, we have it public so that we can call it from a UI element
    public void RaiseArmy()
    {
        PlayerArmy += 3;
    }

	public void RaiseArmy(int i)
	{
		PlayerArmy += i;
	}

    //Another simple function that just closes every UI menu we have open
    public void CloseMenu()
    {
       //However, we want a little delay before we give control back to the player to avoid false input, like the player moving where the button we clicked was
        //I used a coroutine here to show you a different way of doing a timer, instead of the usual way we did it before.
		StartCoroutine("GiveControlToPlayer");
        //Add all menus to close here
    }

    //Our little IEnumerator
   IEnumerator GiveControlToPlayer()
    {
       //Coroutines are pretty powerful but be advised when using them, I'd usually suggest avoiding them altogether unless you have to or you know what you are doing
       //things can get messy pretty fast
        yield return new WaitForSeconds(.5f); //Do note that this is indepedent of the current Time.scale
		 onMenu = false;
    }
}
