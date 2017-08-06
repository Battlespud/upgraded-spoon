using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizationThrusters : ShipComponent {

	//in stabilize mode, they hold us at our current altitude and can be used to counter a leak from the tanks or counteract residual movement forces.
	//in Vertical mode they apply force perpendicular to the forward, ie, they make us go up.  Switch to stabilize to stop moving up, or vent the tanks. Moving vertical consumes double fuel.

	public enum Mode{
		StabilizeY,
		StabilizeXZ,
		Vertical
	}

	public Mode mode = Mode.StabilizeY;

	public const float MaxPower = 100f;
	[Range(0f,1f)]
	public float power;
	public bool Enabled = true;
	Rigidbody shipRB;
	ShipStats shipStats;
	public float YForce;
	public float XForce;
	public float ZForce;

	public float FuelUse;

	// Use this for initialization
	void Start () {
		shipRB = GetComponentInParent<Rigidbody> ();
		shipStats = GetComponentInParent<ShipStats> ();
	}
	//shipStats.GravForce == shipStats.TotalAntiGravForce
	// Update is called once per frame
	void FixedUpdate () {
		FuelUse = 0f;
		if (Enabled && mode == Mode.StabilizeY) {
			FuelUse = .75f * Time.fixedDeltaTime * (power);
			//sanity check for when our acceleration hits super low values.  We cant just clamp it since its a rigidbody, the velocity will break if we try to change it directly.
			if (shipRB.velocity.y > -.001 && shipRB.velocity.y < .001)
				FuelUse = 0f;
				if (shipStats.fuelTank.UseFuel (FuelUse)) {
				YForce = shipRB.velocity.y * -1f * power*MaxPower;
				shipRB.AddRelativeForce (0f, YForce, 0f);
			}
		}
		//not strictly neccessary since we now have drag modeling.  But useful for stopping quicker or when no drag.
		if (Enabled && mode == Mode.StabilizeXZ) {
			FuelUse = 1f * Time.fixedDeltaTime * (power);
			//sanity check for when our acceleration hits super low values.  We cant just clamp it since its a rigidbody, the velocity will break if we try to change it directly.
			if ((shipRB.velocity.x > -.001 && shipRB.velocity.x < .001) && (shipRB.velocity.z > -.001 && shipRB.velocity.z < .001))
				FuelUse = 0f;
			if (shipStats.fuelTank.UseFuel (FuelUse)) {
				XForce = shipRB.velocity.x * -1f * power*MaxPower*1.25f; //horizontal needs a bit more power
				ZForce = shipRB.velocity.z * -1f * power*MaxPower*1.25f;
				shipRB.AddRelativeForce (XForce, 0f, ZForce);
			}


		}
		if (Enabled && mode ==Mode.Vertical) {
			FuelUse = 2f * Time.fixedDeltaTime * (power);
			if (shipStats.fuelTank.UseFuel (FuelUse)) {
				YForce = power*MaxPower;
				shipRB.AddForce (0f, YForce, 0f);
			}
		}
	}
}
