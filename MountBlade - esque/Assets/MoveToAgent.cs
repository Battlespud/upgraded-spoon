using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToAgent : MonoBehaviour {

	//Just a quick testing component for navmeshes.  
	//Agent should automatically try to mvoe towards the destination whenever you set it.

	public GameObject target;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(target)
		agent.SetDestination (target.transform.position);
	}
}
