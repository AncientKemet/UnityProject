using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitManager : Monosingleton<UnitManager>
{

    private Unit[] units;

    void Awake()
    {
        units = new Unit[GlobalConstants.Instance.MAX_UNIT_AMOUNT];
    }

    public void RegisterUnit(Unit unit)
    {
        int id = FreeId;
        unit.ID = id;
        units [id] = unit;
    }
    
    public void DeRegisterUnit(Unit unit)
    {
        if (units [unit.ID] == unit)
        {
            units [unit.ID] = null;
        } else
        {
            Debug.LogError("Broken unit array.");
        }
    }

    public bool WasUnitRegistered(Unit unit)
    {
        return unit.ID != -1;
    }

    public Unit GetUnit(int id)
    {
        return units [id];
    }

    private int FreeId
    {
        get
        {
            for (int i = 0; i < units.Length; i++)
            {
                if (units [i] == null)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
