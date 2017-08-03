using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShipComponent : MonoBehaviour, IDestructible {
	[Header("Ship Component")]
	public bool Functional = true;
	public bool Repairable = true;
	public float CriticalIntegrity = 30f;

	public float Health;
	public float mHealth = 50f;
	[SerializeField] float UIIntegrity;
	public float Integrity { 
		get {return  (Health / mHealth)*100f;	}
	}

	public virtual void Awake(){
		Health = mHealth;
		UIIntegrity = Integrity;
	}

	public virtual void Damage(float dam){
		Health -= dam;
		if (Health < 0f)
			Health = 0f;
		UIIntegrity = Integrity;
		if (Integrity < CriticalIntegrity) 
			Functional = false;
		if (Integrity == 0f)
			Destruct ();
	}

	public virtual void Destruct ()
	{
		Repairable = false;
	}

}
