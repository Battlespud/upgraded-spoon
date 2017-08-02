using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour {

	[SerializeField]
	public List<Rigidbody> ObjectsInWater = new List<Rigidbody>();
	const float GRAVITY = 9.8f;
	float PercentMassPerUnitDepth = .1f;

	public float WaterLevel;

	// Use this for initialization
	void Start () {
		WaterLevel = transform.position.y + .5f * transform.lossyScale.y;
	}

	// Update is called once per frame
	void FixedUpdate () {
		foreach (Rigidbody rb in ObjectsInWater) {
			float depth = WaterLevel - rb.transform.position.y;
			if (depth > 10f)
				depth = 10f; //sanity check

			if(depth > 1)
			rb.AddForce(Vector3.up*((rb.mass*GRAVITY)*(PercentMassPerUnitDepth*depth)));
		}
	}

	void OnTriggerEnter(Collider other){
		Rigidbody rb = other.GetComponent<Rigidbody> ();
		if(rb)
			ObjectsInWater.Add (rb);
	}

	void OnTriggerExit(Collider other){
		if (other.GetComponent<Rigidbody> ())
			ObjectsInWater.Remove (other.GetComponent<Rigidbody> ());
	}

}
