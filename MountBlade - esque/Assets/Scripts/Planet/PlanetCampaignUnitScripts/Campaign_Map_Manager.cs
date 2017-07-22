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
    public List<FactionsBase> FactionList = new List<FactionsBase>();

    public GameObject VillageUI;//The UI for when the player is visiting a village
    //Do note, for my projects, I usually keep everything that has to do with the UI on a different script to avoid confusions

	public string PlayerName;
	public InputField nameInput;
	public GameObject InputUI;



    void Awake()
    {
		PreventDuplicationAndDontDestroy ();
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
		StartCoroutine ("AskPlayerName");
    }

	void NewGame(){
		SceneManager.LoadScene ("Space");
		InitializeSpace ();
	}

	void InitializeSpace (){
		VillageUI = GameObject.FindGameObjectWithTag ("VillageUI");
		VillageUI.SetActive(false);
	}

	void InitializePlanet (){
		VillageUI = GameObject.FindGameObjectWithTag ("VillageUI");
		VillageUI.SetActive(false);
	}

	void InitializeCity (){
		VillageUI = GameObject.FindGameObjectWithTag ("VillageUI");
		VillageUI.SetActive(true);
	}

	void InitializeBattle (){

	}

	IEnumerator AskPlayerName(){
		bool nameEntered = false;
		nameInput.ActivateInputField ();
		nameInput.Select();
		while (!nameEntered) {
			if (nameInput.text != "" && Input.GetKeyDown(KeyCode.Return)) {
				nameEntered = true;
			}
			yield return null;
		}
		PlayerName = nameInput.text;
		Debug.Log (PlayerName);
		InputUI.SetActive (false);
		NewGame ();
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
        VillageUI.SetActive(false);
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

//See below for more
[System.Serializable]
public struct FactionsBase
{
    public int factionId;
    public Color factionColor;
    
    public int startingCharacters;
    public GameObject characterPrefab;

    public int currentArmyUnits;
    public int storePreviousArmyUnits;
    public int targetArmyUnits;
    
    public int factionWealth;
    public int factionWealthTarget;
    
    public List<int> EnemiesWith;
    public List<CampaignMap_AIUnit_Planet> FactionCharacters;
    public List<CampainMap_POI> FactionCastles;
    public List<CampainMap_POI> FactionVillages;

    public FactionAIstate factionAIstate;
    public FactionTraits factionTraits;
    public FactionPrefers factionPrefers;

    public enum FactionAIstate
    {
        GoToWar,RaiseArmies,RaiseResources
    }

    public enum FactionTraits
    {
        Aggresive,Passive,Diplomatic
    }

    public enum FactionPrefers
    {
        Oranges,Apples,Lemons,Cake
    }
}
/* This is a struct
 * If you haven't used one before, you are lying :P
 * The majority of what you use is basically a struct*, as the example I gave in the video
 * a Vector3 is basically this:
 * 
 * public struct Vector3
 * { 
 *   float x;
 *   float y;
 *   float z;
 * }
 * 
 * Simple aint it? Now do take it with a grain of salt since I haven't actually seen the source code behind it but I can imagine,
 * If you where learning C++ (or even C# for that matter) outside of a game engine enviroment, this would have been one of the first(est) things you would learn
 * 
 */