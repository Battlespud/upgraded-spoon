using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechModule : MonoBehaviour {

	//used to announce speech.  Don't place this manually, its setup to be used as part of the speechblock prefab so just use that.  


	Transform cameraTransform;
	float fov;

	Canvas SpeechBubble;
	Text text;

	static List<int> SpeakerIDList = new List<int> ();

	[SerializeField]private int speakerID = 0;

	[SerializeField]private int speechStage = 0;

	[SerializeField]private int mSpeechStage = 0;

	public bool locked = true;
	public bool triggered = false;


	public bool SlaveMode = false;

	// Use this for initialization
	void Start () {
		SpeechBubble = gameObject.GetComponentInChildren<Canvas> ();
		SpeechBubble.enabled = false;

		text = gameObject.GetComponentInChildren<Text> ();
		text.enabled = false;

	}
	
	void Update () {

	}

	void AdvanceSpeechStage(){
		speechStage++;
		if (speechStage > mSpeechStage) {
			speechStage = 0;
		}
	}

	void Speak(){
		text.text = SpeechRepo.RetrieveSpeech (new SpeechKey (speakerID, speechStage));
	}

	public void Speak(SpeechKey key){
		Enable ();
		text.text = SpeechRepo.RetrieveSpeech (key);
	}

	public void Unspeak(){
		Disable ();
	}

	void Enable(){
		SpeechBubble.enabled = true;
		text.enabled = true;
	}

	void Disable(){
		SpeechBubble.enabled = false;
		text.enabled = false;
	}

	public void Enslave(){
		SlaveMode = true;
		gameObject.GetComponent<SphereCollider> ().enabled = false;
	}

	public void Free(){
		SlaveMode = false;
		gameObject.GetComponent<SphereCollider> ().enabled = true;
	}

	void OnTriggerEnter(Collider col){
		if (col.gameObject.CompareTag ("Player") && !triggered && !SlaveMode) {
			Speak ();
			Enable ();
			triggered = true;

		}
	}

	void OnTriggerExit(Collider col){
		if (col.gameObject.CompareTag ("Player") && !SlaveMode) {
			triggered = false;
			Disable();
			AdvanceSpeechStage ();
		}
	}



}
