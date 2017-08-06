using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

	//Overall controller for the UI. Work in progress, but functional rn..


	//UI Blocks
	public GameObject MainMenu;
	public GameObject VillageUI;//The UI for when the player is visiting a village
	public GameObject InputUI;  //The UI for when the player is entering name
	public GameObject FactionAndRace;
	public GameObject SplashScreen;
	public GameObject RecruitmentUI;


	public List<AudioClip> AudioList;


	Campaign_Map_Manager MapManager;


	public string PlayerName;
	public InputField nameInput;

	public AudioSource Audio;

	public List<GameObject> UI_Elements;


	void Awake()
	{
		UI_Elements = new List<GameObject>();
	}



	// Use this for initialization
	void Start () {
		UI_Elements = new List<GameObject> ();
		Audio = GetComponent<AudioSource> ();
		MapManager = GetComponent<Campaign_Map_Manager> ();
		DisableAllUI ();
		StartCoroutine ("ShowSplashScreen");

	}

	public void DisableAllUI(){
		MainMenu.SetActive (false);
		FactionAndRace.SetActive (false);
		SplashScreen.SetActive (false);
		VillageUI.SetActive (false);
		InputUI.SetActive (false);
		RecruitmentUI.SetActive (false);
	}

	IEnumerator ShowSplashScreen(){
		SplashScreen.SetActive (true);
		Audio.clip = AudioList [0];
		Audio.Play ();
		bool proceed = false;
		while (!proceed) {
			if (Input.anyKeyDown)
				proceed = true;
			yield return null;
		}
		//StartAskPlayerName ();
		StartMainMenu();

	}

	void StartMainMenu(){
		MainMenu.SetActive (true);
		UIBugFix ();

	}



	public void RecieveChangeSceneEvent(string scene){
		PurgeUI ();
		switch (scene) {
		case "Space":
			{
				VillageUI.SetActive(false);

				break;
			}

		case "Planet":
			{
				VillageUI.SetActive (false);
				break;
			}
		case "City":
			{
				VillageUI.SetActive (false);

				break;
			}
		case "VillageBattle":
			{
				VillageUI.SetActive (false);

				break;
			}
		case "AirBattle":
			{
				VillageUI.SetActive (false);

				break;
			}
		}

	}

	//Literally dont ask. Just call it whenever you enable something besides the splashscreen.
	public void UIBugFix(){
		SplashScreen.SetActive (false);
		SplashScreen.SetActive (true);
		SplashScreen.SetActive (false);
	}

	public void EnableVillageUI(){
		DisableAllUI ();
		VillageUI.SetActive (true);
		UIBugFix ();
	}

	public void DisableVillageUI(){
		VillageUI.SetActive (false);
		MapManager.CloseMenu ();
	}

	public void EnableRecruitmentUI(){
		DisableAllUI ();
		RecruitmentUI.SetActive(true);
		UIBugFix ();
	}

	public void DisableRecruitmentUI(){
		RecruitmentUI.SetActive (false);
		EnableVillageUI ();
	}
	void PurgeUI(){
		for(int i = 0; i < UI_Elements.Count; i++){
			GameObject.Destroy (UI_Elements [i]);
		}
	}

	public void StartAskPlayerName(){
		DisableAllUI ();
		StartCoroutine ("AskPlayerName");
	}

	IEnumerator AskPlayerName(){
		InputUI.SetActive (true);
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
		Debug.Log ("Player name set to " + PlayerName);
		DisableAllUI ();
		FactionAndRace.SetActive (true);
		UIBugFix ();
	}



	public void ExitGame(){
		Application.Quit ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
