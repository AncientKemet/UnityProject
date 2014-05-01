using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{

  private int _id = -1;

  public int ID
  {
    get
    {
      return -1;
    }
    set
    {
      if (UnitManager.Instance.WasUnitRegistered(this))
      {
        UnitManager.Instance.DeRegisterUnit(this);
      }

      _id = value;

      UnitManager.Instance.RegisterUnit(this);
    }
  }

}
