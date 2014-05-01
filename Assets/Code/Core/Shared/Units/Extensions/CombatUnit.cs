using UnityEngine;
using System.Collections;

/// <summary>
/// Destroyable Unit extension.
/// </summary>
public class CombatUnit : MoveableUnit
{

  [SerializeField]
  private float _baseHealth = 100f;

  [SerializeField] 
  private float _baseArmor = 0;

  [SerializeField] 
  private float _baseMagicResist = 0;

  private float _health;

  public void AppendHealthChange(HealthChange _change)
  {
    if (_change is Damage)
    {
      _health -= _change.value;
    } 

    if(_change is Heal)
    {
      _health += _change.value;
    }
  }



}
