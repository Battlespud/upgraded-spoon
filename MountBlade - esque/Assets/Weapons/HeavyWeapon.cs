using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeavyWeapon : MonoBehaviour {


	//Position of shots
	public GameObject Muzzle;

	//Sound
	public AudioClip FireSound;
	public AudioClip ReloadSound;

	public AudioSource WeaponSoundSource;

	public Light MuzzleFlash;

	public bool UnlimitedAmmo = true;


	public bool Safety = false; //wont fire at friends.

	public virtual void LoadReferences(){

		//Muzzle


		//Light
		MuzzleFlash = GetComponentInChildren<Light> ();
		MuzzleFlash.enabled = false;

		//Sound
		WeaponSoundSource = GetComponent<AudioSource>();
		//Debug.Log (string.Format ("Sounds/Weapons/{0}/{1}FireSound", name, name));
		FireSound = Resources.Load<AudioClip> (string.Format("Sounds/Weapons/{0}/{1}FireSound",name ,name));
		ReloadSound = Resources.Load<AudioClip> (string.Format("Sounds/Weapons/{0}/{1}ReloadSound",name,name,ReloadSound.ToString()));

	}


	public virtual void Reload(){
		if (ammo != maxAmmo && clips > 0 && !reload) {
			reload = true;
			ammo = 0;
			reloadTimer = reloadTime;
			try {
				WeaponSoundSource.PlayOneShot (ReloadSound);
			} catch {
				Debug.Log ("This weapon needs a reload sound. " + name);
			}
		}
	}

	public virtual void Fire(){
		if (ammo > 0 && canFire) {
			try{WeaponSoundSource.PlayOneShot (FireSound);}catch{Debug.Log ("This weapon needs a firing sound. " + name);}
			ammo--;
			OnFire ();
		}
		Debug.Log ("You need to write a custom fire method for this weapon still");
	}

	public virtual void OnFire(){
		MuzzleFlash.enabled = true;
		Invoke ("EndMuzzleFlash", cooldown);
	}

	public virtual void HandleCooldown(){
		canFire = false;
		Invoke("CheckCooldown",cooldown);
	}

	public virtual void CheckCooldown(){
		canFire = true;
	}

	public void EndMuzzleFlash(){
		MuzzleFlash.enabled = false;
	}

	public string name;


	public float maxRange;

	public int damage;
	public int maxAmmo;
	public int ammo;
	public int clips;
	public int maxClips;
	public float accuracy;

	public bool automatic = true;
	bool reload = false;

	//between shots
	public float cooldownTimer;
	public float cooldown;
	public bool canFire=true;


	public float reloadTime;
	public float reloadTimer;



	public virtual void AimDownSights(){
		//TODO
	}


	public void Update () {

		if (reload) {
			if (reloadTimer > 0) {
				reloadTimer -= Time.deltaTime;
			} else if (reloadTimer <= 0) {
				//	Debug.Log ("Loaded!");
				ammo = maxAmmo;
				clips -= 1;
				reload = false;
			}
		}

		Debug.DrawRay (Muzzle.transform.position, Muzzle.transform.forward * maxRange, Color.yellow);

	}
}
