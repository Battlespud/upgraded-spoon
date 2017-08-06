using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechMaster : MonoBehaviour {

	//use for large groups of mooks to prevent a bunch of triggers floating around.
		//just for testing, we can change these anytime ofc
	public	int speakerID = 1;
	public	int speechID = 2;

	public bool active = false;

	List<SlaveSpeechModule> Slaves;

	public void register(SlaveSpeechModule slave){
		Slaves.Add(slave);
	}

	// Use this for initialization
	void Awake () {
		Slaves = new List<SlaveSpeechModule> ();
	}


	public void MassSpeak(SpeechKey k){
		foreach(SlaveSpeechModule s in Slaves){
			s.Speak (k);
		}
	}

	public void MassUnspeak(){
		foreach(SlaveSpeechModule s in Slaves){
			s.Unspeak ();
		}
	}

	void OnTriggerEnter(Collider col){
		if(col.gameObject.CompareTag("Player")){
			active = true;
			MassSpeak (new SpeechKey (speakerID, speechID));
		}
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.CompareTag("Player")){
			active = false;
			MassUnspeak ();
		}
	}




	
	// Update is called once per frame
	void Update () {
		
	}
}
