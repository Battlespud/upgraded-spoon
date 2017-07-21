using UnityEngine;
using System.Collections;

public class CampaignMap_PlayerUnit : MonoBehaviour {

    /*This script belongs to the players unit on the campaign map
     */

    UnityEngine.AI.NavMeshAgent agent; 
    bool hasDestination; //if we have a destination
    Vector3 movePosition; //where we are going

    //The destination type we are going to
    public CampainMap_POI.POItype destinationType;

    CampainMap_POI cmDetails;//our own POI details
    Campaign_Map_Manager cmManager;

	void Start () 
    {
        //Get the references
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        cmManager = GameObject.FindGameObjectWithTag("CampaignMapManager").GetComponent<Campaign_Map_Manager>();
        cmDetails = GetComponent<CampainMap_POI>();      
	}
		
	void Update () 
    {
        cmDetails.UnitListNumber = cmManager.PlayerArmy; //How many units the player has is controlled by the campaign map manager

        //When we click
	    if(Input.GetMouseButtonUp(0))
        {
            if (!cmManager.onMenu) //And not on a menu
            {
                MoveToPosition();//The player wants to move
            }
        }

        if(hasDestination)//If we have a destination
        {
            agent.SetDestination(movePosition);//Move there
            
            //Check the distance
            float distance = Vector3.Distance(transform.position, movePosition);

            if(distance < 2)
            {
                //And if we reached our destination
                CheckDestination();//Check what type of destination was and do what you have to do
                hasDestination = false;//we no longer have a destination
            }
        }
	}

    void CheckDestination()
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
                Application.LoadLevel(1);
                break;
            case CampainMap_POI.POItype.Unit:
                //Load battlefield
                break;
            case CampainMap_POI.POItype.Village:
                //Load village UI
                cmManager.onMenu = true;
                cmManager.VillageUI.SetActive(true);
                break;
        }
    }

    void MoveToPosition()
    {
        //Do a raycast from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if(Physics.Raycast(ray,out hit, Mathf.Infinity))
        {
            //If we hit something which is a POI
            if(hit.transform.GetComponent<CampainMap_POI>())
            {
                //store that POI component
                CampainMap_POI cPOI = hit.transform.GetComponent<CampainMap_POI>();

                //We want to move there, but not where the raycast hit, we want to move to it's origin
                movePosition = hit.transform.position;
                cmManager.currEnemies = cPOI.UnitListNumber; //Update the enemies the player will face if he goes there (and if it's actually an enemy/battlefield)
                destinationType = cPOI.Point_of_Interest_Type;//And change our destination type
            }
            else //If it's not a POI that probably means it's an empty ground
            {
                cmManager.currEnemies = 0;//no enemies there
                movePosition = hit.point;//where we want to move
                destinationType = CampainMap_POI.POItype.Terrain; //Do note that terrain is actually a POI type, but we never assign a component to that
                //But even if you do assign it, it won't be an error, since we ovverride the type anyway
            }

            hasDestination = true; //We have a destination, hurray!
        }
    }
}
