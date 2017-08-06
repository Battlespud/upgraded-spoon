﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Activator : MonoBehaviour {
	//used to activate objects implementing IActivate. Stick it on a player prefab.
	//Dependent on having a UI properly setup, which this project doesnt yet. This was copied directly from our other proj.



	Ray activationRay;
	public const float ActivationDistance = 1f;
	public Text ActivationToolTipText;
	string emptyString;

	// Use this for initialization
	void Start () {
		emptyString = ("");
	}
	// Update is called once per frame
	void Update () {
//		activationRay = new Ray (transform.position, transform.forward);
			RaycastHit hit;
		try{
		if (Physics.Raycast (transform.position, transform.forward, out hit, ActivationDistance)) {
			if (hit.collider.GetComponent<IActivate> () != null) {
				IActivate active = hit.collider.GetComponent<IActivate> ();
				if (Input.GetKeyDown (KeyCode.F)) {
					active.Activate ();
				} else {
					ActivationToolTipText.text = active.GetToolTip ();
				}
			} else {
				ActivationToolTipText.text = emptyString;
			}
		} else {
			ActivationToolTipText.text = emptyString;
		}
	}
	catch{
		//idc
	}
}
}
