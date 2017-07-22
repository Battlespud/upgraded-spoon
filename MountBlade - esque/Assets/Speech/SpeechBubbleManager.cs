using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubbleManager : MonoBehaviour {

	Transform cameraTransform;
	float fov;

	Canvas SpeechBubble;

	Vector3 bScale;

	// Use this for initialization
	void Start () {
		SpeechBubble = gameObject.GetComponentInChildren<Canvas> ();

		bScale = SpeechBubble.transform.localScale; //ideal for fov 30 as originally setup

	}




	void Update () {
		//face camera 
		if (SpeechBubble.enabled) {
			cameraTransform = Camera.main.transform; 
			fov = Camera.main.fieldOfView; //for text sizing
			Quaternion tempternion = Quaternion.LookRotation (transform.position - cameraTransform.position);
			float y = tempternion.eulerAngles.y;
			//	Debug.Log (y);
			//	SpeechBubble.transform.eulerAngles = new Vector3(220f, y, transform.rotation.z);
			setScale ();
		}
	}

	void setScale(){
		float a = 1;

			a	= (float) fov / 30 + .4f;
		SpeechBubble.transform.localScale = bScale * a;
	}

}
