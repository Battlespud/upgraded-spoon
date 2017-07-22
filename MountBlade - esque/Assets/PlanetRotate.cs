using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetRotate : MonoBehaviour {

	public float speed = 2.5f;

	
	// Update is called once per frame
	void Update () {
		transform.Rotate (transform.up * speed * Time.deltaTime);
	}
}
