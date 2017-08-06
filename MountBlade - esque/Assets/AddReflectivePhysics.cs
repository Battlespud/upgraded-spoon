using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddReflectivePhysics : MonoBehaviour {


	//On scene start, adds reflective physics material to any gameobject missing it that has the proper tag.  Deletes itself afterwards.  

	public PhysicMaterial reflectiveMat;
	List<GameObject> gameobjects;

	// Use this for initialization
	void Start () {

		gameobjects = new List<GameObject>(GameObject.FindGameObjectsWithTag ("Reflective"));
		foreach (GameObject g in gameobjects) {
			Collider c = g.GetComponent<Collider> ();
			c.material = reflectiveMat;
		}
		GameObject.Destroy (this);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
