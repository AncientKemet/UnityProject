using UnityEngine;
using System.Collections;

// author paris stefanou

public class Unit : movesimple{

	public void movemetupdate (float speed,float slow) {
		movementSpeed=speed*(1-slow);
		targetPosition = transform.localPosition;
	}

	public int ID {
		get;
		set;
	}

	void Awake(){
		UnitManager.Instance.RegisterUnit(this);
	}


}
