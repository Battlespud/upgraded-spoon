using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDissolveScript : MonoBehaviour {

	Material mat;
	Renderer ren;

	public bool running = false;

	float f;
	float burnSize = .18f;
	// Use this for initialization
	void Start () {
		ren = GetComponent<Renderer> ();
		mat = ren.material;

	}
	
	// Update is called once per frame
	void Update () {
		if (running) {
			f = Mathf.Lerp (f, 1f, .125f * Time.deltaTime);
			mat.SetFloat ("_SliceAmount", f);
		}
	}
}
