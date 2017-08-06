using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldCameraController : MonoBehaviour {

	//just stick it on the camera in the planet and space scenes.

	const float BaseSensitivity = 5;
	float Sensitivity;
	Vector3 offset;

	// Use this for initialization
	void Start () {
		Sensitivity = BaseSensitivity;
	}
	
	// Update is called once per frame
	void Update () {
		InputLoop ();

		transform.position = new Vector3 (transform.position.x + offset.x, transform.position.y, transform.position.z + offset.z);
	}

	void InputLoop(){
		offset = new Vector3 ();
		if(Input.GetKey (KeyCode.LeftShift)){
			Sensitivity = BaseSensitivity *2.5f;
		}
		else{
			Sensitivity = BaseSensitivity;
		}
		offset.z = Input.GetAxis ("Vertical") * Sensitivity;
		offset.x = Input.GetAxis ("Horizontal") * Sensitivity;


	}

}

