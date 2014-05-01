using UnityEngine;
using System.Collections;

/// <summary>
/// Destroyable Unit extension.
/// </summary>
public class DestroyableUnit : Unit
{

  [SerializeField]
  protected float _baseHealth = 100f;
  private float _health;

  public void AppendHealthChange(HealthChange _change)
  {
    if (_change is Damage)
    {
      //process some changes ie. _change.value *= armorreduciton

      _health -= _change.value;
    } 
  }

}
