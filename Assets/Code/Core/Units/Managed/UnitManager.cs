using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : Monosingleton<UnitManager>
{

	private Unit[] units;

	void Awake(){
		units = new Unit[GlobalConstants.Instance.MAX_UNIT_AMOUNT];
	}

	public void RegisterUnit (Unit unit)
	{
		if (units [unit.ID] == null) {
			units [unit.ID] = unit;
			Debug.Log("Registered unit["+unit.ID+"]. "+unit);
		} else {
			Debug.LogError ("Broken unit array.");
		}
	}
	
	public void DeRegisterUnit (Unit unit)
	{
		if (units [unit.ID] == unit) {
			units [unit.ID] = null;
		} else {
			Debug.LogError ("Broken unit array.");
		}
	}

	public Unit GetUnit(int id){
		return units[id];
	}
}
