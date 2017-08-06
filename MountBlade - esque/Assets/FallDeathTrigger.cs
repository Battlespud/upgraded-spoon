using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallDeathTrigger : MonoBehaviour {
	// Stick on a  big square trigger underneath the minimum altitude on air maps so people die when they fall off.


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.GetComponent<CharacterStats> ()) {
			CharacterStats stats = other.GetComponent<CharacterStats> ();
			stats.DoDamage (stats.Health * 2f);
		}
	}
}
