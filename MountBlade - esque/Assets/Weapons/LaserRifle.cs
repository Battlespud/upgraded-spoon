using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserRifle : Weapon {

	const float LaserTrail = .125f; //default .175



	public GameObject lrPrefab;

	public bool useTrueAim = false;


	//sanity check, prevents infinite loop
	int maxBounces = 6;
	int bounces = 0;


	// Use this for initialization
	void Start () {
		name = "LaserRifle";
		maxRange = 100;
		damage = 500;
		maxAmmo = 20;
		ammo = 0;
		maxClips = 12;
		clips = maxClips;
		accuracy = 1f;
		cooldown = .1f; //.3
		reloadTime = 1.4f;
		canFire = true;
		Safety = true;
		automatic = true;

		LoadReferences ();
	}

	public override void AimDownSights ()
	{
		throw new System.NotImplementedException ();
	}

	#region shooting
	public override void Fire(){
		bounces = 0;
		if (!canFire || ammo <= 0) {
			return;
		}
		if (!FireControl())
			return;
	//	Debug.DrawRay (Muzzle.transform.position, cam.ScreenPointToRay (Input.mousePosition).direction * maxRange, Color.green, 3f);
	//	Debug.DrawRay (Muzzle.transform.position, Muzzle.transform.forward * maxRange, Color.red);

		RaycastHit hit;
		Ray firingRay;
		if (useTrueAim) {
			firingRay = new Ray (Muzzle.transform.position, Muzzle.transform.forward*maxRange);
		} else {
			
			firingRay = new Ray (Muzzle.transform.position, cam.ScreenPointToRay (Input.mousePosition).direction);
		}
		ammo--;
		OnFire ();
		HandleCooldown ();
		WeaponSoundSource.PlayOneShot (FireSound);
		if (Physics.Raycast (firingRay, out hit, maxRange)) {
			if (hit.collider.gameObject.GetComponent<CharacterStats> ()) {
				hit.collider.GetComponent<CharacterStats>().DoDamage(damage);
				if (hit.collider.GetComponent<Rigidbody> ()) {
					//hit.collider.GetComponent<Rigidbody> ().AddExplosionForce (300000f, hit.point, 10f);
				}
				//DissolveEffect targetEffect = hit.collider.gameObject.AddComponent<DissolveEffect> ();
			////	targetEffect.TriggerDissolve (hit.point);
				StartCoroutine (LineRendererHandlerFirstShot (hit.point));
			} else {
				if (hit.point != null) {
					Debug.Log (hit.collider.name);
					StartCoroutine (LineRendererHandlerFirstShot (hit.point));
					StartCoroutine (LineRendererHandlerFirstShot (hit.point));
					if(hit.collider.gameObject.CompareTag("Reflective")){
						BounceShot (hit, firingRay);
					}
				}
			}
		} else {
			if (!useTrueAim) {
				StartCoroutine (LineRendererHandlerFirstShot (Muzzle.transform.position + (cam.ScreenPointToRay (Input.mousePosition).direction * maxRange)));
			} else {
				StartCoroutine (LineRendererHandlerFirstShot (Muzzle.transform.position + (Muzzle.transform.forward * maxRange)));
			}
		}

	}

	void BounceShot(RaycastHit hit, Ray firingRay){
		if (bounces > maxBounces) {
			return;
		}
		bounces++;
	//	Debug.Log ("Bounce laser");
	//	Debug.DrawRay (hit.transform.position, Vector3.Reflect(firingRay.direction,hit.normal)*maxRange, Color.red, 3f);
		Ray newFiringRay = new Ray(hit.transform.position, Vector3.Reflect(firingRay.direction,hit.normal));
		RaycastHit newHit;
		if (Physics.Raycast (newFiringRay, out newHit, maxRange)) {
			if (newHit.collider.gameObject.GetComponent<CharacterStats> ()) {
				newHit.collider.GetComponent<CharacterStats>().DoDamage(damage);
				StartCoroutine (LineRendererHandler (hit.point, newHit.point));
			} else {			//not CharacterStats
				if (newHit.point != null) {
					StartCoroutine (LineRendererHandler (hit.point,newHit.point));
					if (hit.collider.gameObject.CompareTag ("Reflective")) {
						BounceShot (newHit, newFiringRay);
					}
				} else {
					StartCoroutine (LineRendererHandler (hit.point, Vector3.Reflect (firingRay.direction, hit.normal) * maxRange));
				}
			}
		} else {
			StartCoroutine (LineRendererHandler (hit.point, Vector3.Reflect (firingRay.direction, hit.normal) * maxRange));

		}
//		Debug.Log (bounces);
	}
	#region Coroutines
	private IEnumerator LineRendererHandlerFirstShot(Vector3 endPos){
		MuzzleFlash.enabled = true;
		GameObject renderer = Instantiate (lrPrefab);
		LineRenderer lr = renderer.GetComponent<LineRenderer> ();
		MuzzleFlash.color = Color.cyan;
		lr.startColor = Color.cyan;
		lr.endColor = Color.cyan;
		renderer.transform.position = Muzzle.transform.position;
		float time = LaserTrail;
		float elapsedTime = 0f;
		lr.enabled = true;
		lr.SetPositions(new Vector3[2]{Muzzle.transform.position,endPos});
		while (elapsedTime < time) {
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		MuzzleFlash.enabled = false;
		Destroy (renderer);
	}

	private IEnumerator LineRendererHandler(Vector3 startPos, Vector3 endPos){
		//Debug.Log ("Bounce laser");
		GameObject renderer = Instantiate (lrPrefab);
		LineRenderer lr = renderer.GetComponent<LineRenderer> ();
		lr.startColor = Color.red;
		lr.endColor = Color.red;
		float time = LaserTrail;
		float elapsedTime = 0f;
		lr.enabled = true;
		lr.SetPositions(new Vector3[2]{startPos,endPos});
		while (elapsedTime < time) {
			elapsedTime += Time.deltaTime;
			yield return null;
		}
		Destroy (renderer);
	}
	#endregion
	#endregion


}
