using UnityEngine;
using UnityEngine.UI; //Don't forget me!
using System.Collections;

public class CampainMap_POI : MonoBehaviour {
    /*Every gameobject on the map, is actually a Point of Interest
     * some are more interesting than others though...
     */

    public GameObject UI_Element_Prefab; //The prefab for UI element we want to instantiate (Basically the text)
    GameObject UI_Element; //We want to store somewhere that instatiated prefab
    Canvas canvas;//Reference to our canvas
    public Campaign_Map_Manager cmManager; //Our campaign map manager
	public UIManager uiManager;

    public int FactionNumber; //To what faction this POI belongs to
    public Text UnitListTextAsComponent;//The text under the POI
    public int UnitListNumber = 1; //How many enemies this POI holds
    Color factionColor; //We store the faction color here

    //What type of POI this is
    public POItype Point_of_Interest_Type;

    public enum POItype
    {
        Player,
        Unit,
        Village,
        Castle,

		//new ones
		Spaceport,
		Planet,


        Terrain //Note, we never assign a POI with a terrain type
    }

    public CampainMap_POI HomeBase;

    void Start()
    {
        //Find our references
        cmManager = GameObject.FindGameObjectWithTag("CampaignMapManager").GetComponent<Campaign_Map_Manager>();
		canvas = cmManager.GetComponentInChildren<Canvas>();
		uiManager = cmManager.gameObject.GetComponent<UIManager> ();
        //Instatiate our UI element and store it
        UI_Element = Instantiate(UI_Element_Prefab, transform.position, Quaternion.identity) as GameObject;
        UI_Element.transform.SetParent(canvas.transform); //For UI elements, you can't use transform.parent = , you have to use .SetParent()
        UnitListTextAsComponent = UI_Element.GetComponentInChildren<Text>();

		uiManager.UI_Elements.Add (UI_Element);
        //Initialize the color here
        if (Point_of_Interest_Type != POItype.Player)
        {
            //If it's not a player, then it's faction's color from the campaign manager script
            factionColor = cmManager.FactionList[FactionNumber].factionColor;
        }
        else
        {
            //If it's a player just give him a white color
            factionColor = Color.white;
        }

        //Change the color
        UnitListTextAsComponent.color = factionColor;

        //Add the POI to the correct list
        InitializeFactionOwner();
    }

    void InitializeFactionOwner()
    {
        switch (Point_of_Interest_Type)
        {
            case POItype.Unit:
                cmManager.FactionList[FactionNumber].FactionCharacters.Add(GetComponent<CampaignMap_AIUnit_Planet>());
                break;
            case POItype.Village:
                cmManager.FactionList[FactionNumber].FactionVillages.Add(this);
                break;
            case POItype.Castle:
                cmManager.FactionList[FactionNumber].FactionCastles.Add(this);
                break;
        }
        
    }

    void Update()
    {
		if (UI_Element != null) {
			UIFollowObject ();
			UpdateUIproperties ();
		}
    }

    //Update the text of the UI element based on the type and add the number of units
    void UpdateUIproperties()
    {
        switch(Point_of_Interest_Type)
        {
            case POItype.Player:
			UnitListTextAsComponent.text = cmManager.PlayerName + "(" + UnitListNumber + ")";
                break;
            case POItype.Unit:
                UnitListTextAsComponent.text = "Unit " + "(" + UnitListNumber + ")";
                break;
            case POItype.Castle:
                UnitListTextAsComponent.text = "Castle " + "(" + UnitListNumber + ")";
                break;
            case POItype.Village:
                UnitListTextAsComponent.text = "Village " + "(" + UnitListNumber + ")";
                break;
			case POItype.Spaceport:
				UnitListTextAsComponent.text = "Spaceport " + "(" + UnitListNumber + ")";
				break;
			case POItype.Planet:
				UnitListTextAsComponent.text = "Planet " + "(" + UnitListNumber + ")";
				break;
        }
    }

    //This functions make the UI element to follow a 3d object on the screen   
    void UIFollowObject()
    {
        //Follow a 3d Object
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        UI_Element.transform.position = screenPoint;
        //psss.. this is useful a lot!
    }
}
