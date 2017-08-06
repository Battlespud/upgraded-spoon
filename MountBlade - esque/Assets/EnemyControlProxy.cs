using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControlProxy : MonoBehaviour {

	//Used for getting around navmesh limitations.

	Weapon weapon;
	public CharacterController Controller;

	public GameObject ShipReference;

	public EnemyControl RealEnemyControl;

	// Use this for initialization
	 void Start () {
		Controller =GetComponent<CharacterController> ();
		transform.SetParent (ShipReference.transform);
		RealEnemyControl.inMirrorMode = true;
	}
	
	// Update is called once per frame
	 void Update () {
		
	}

	 void LateUpdate () {

	}


	public void MirrorMove(Vector3 input){
		Controller.SimpleMove (input);
	}

	public void Fire(){
		weapon.Fire ();
	}

	public void SetRotation(Quaternion quat){
		transform.rotation = quat;
	}

	public void SetRealEnemyControl(EnemyControl cha){
		RealEnemyControl = cha;
	}
}
