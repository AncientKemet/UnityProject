using UnityEngine;
using System.Collections;

// author paris stefanou

public class Unit : movesimple{

	private int _id = -1;

	public int ID {
		get{
			return -1;
		}
		set{
			if(UnitManager.Instance.WasUnitRegistered(this)){
				UnitManager.Instance.DeRegisterUnit(this);
			}

			_id = value;

			UnitManager.Instance.RegisterUnit(this);
		}
	}


}
