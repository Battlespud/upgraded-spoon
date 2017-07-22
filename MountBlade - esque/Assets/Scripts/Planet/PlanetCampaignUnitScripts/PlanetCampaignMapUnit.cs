using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlanetCampaignMapUnit : MonoBehaviour {

	//this is public so that we can access it without having to use GetComponent everytime. Our own details.
	public CampainMap_POI cmPOI; 

	//Used to actually move the blocks around. WILL WALK THROUGH WALLS IF NOT SET TO STATIC!!!
	public UnityEngine.AI.NavMeshAgent agent;

	//Where we are trying to go.  Handling is done in child class.
	public Vector3 targetDestination;

	//Do we have somehwere to go?
	public bool hasDestination;


}
