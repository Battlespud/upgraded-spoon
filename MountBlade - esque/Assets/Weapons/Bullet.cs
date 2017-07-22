using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	void OnCollisionEnter(Collision col){
		if(col.collider.gameObject.GetComponent<CharacterStats>()!=null){
		col.collider.gameObject.GetComponent<CharacterStats>().DoDamage(3);
		}
		else
		if(col.collider.gameObject.GetComponentInParent<CharacterStats>()!=null){
			col.collider.gameObject.GetComponentInParent<CharacterStats>().DoDamage(3);
		}
		else
		if(col.collider.gameObject.GetComponentInChildren<CharacterStats>()!=null){
			col.collider.gameObject.GetComponentInChildren<CharacterStats>().DoDamage(3);
		}
		if(col.gameObject.CompareTag("Reflective")){
			return;
		}
		Debug.Log (col.gameObject.name);
		Destroy(gameObject);

	}




}
