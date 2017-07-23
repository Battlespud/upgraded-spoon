using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordDamage : MonoBehaviour {

	public EnemyControl control;
	public PlayerControl pControl;
	Animator anim;
	float damage = 100f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider co){
		Debug.Log ("Sword hit!");
		if (co.GetComponent<CharacterStats> () || co.GetComponentInParent<CharacterStats> ()) {
			CharacterStats cha = co.GetComponent<CharacterStats> ();
			if (cha == null)
				cha = co.GetComponentInParent<CharacterStats> ();
			if (control != null) {
				//if (control.attacking)
				cha.DoDamage (damage);
			} else if (pControl.inAttackAnimation) {
				cha.DoDamage (damage);
			}
		}
	}
}
