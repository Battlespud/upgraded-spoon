using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible {

	//for any physical nonplayer object that should take damage on hit. 

	 void Damage (float dam);
	 void Destruct();

}
