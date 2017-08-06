using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : ShipComponent {

	//Not implemented yet. Waiting on a heavy weapon to test with.

	System.Type WeaponType;
	public int MaxAmmo = 20;
	public int Ammo;

	public bool UseAmmo(HeavyWeapon h){
		if (h.GetType () != WeaponType)
			return false;
		if (Ammo == 0)
			return false;
		Ammo -= 1;
		return true;
	}

	// Use this for initialization
	void Start () {
		Ammo = MaxAmmo;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
