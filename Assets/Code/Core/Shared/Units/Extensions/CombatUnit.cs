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
	private float _baseresource = 100f;

	[SerializeField] 
  private float _baseArmor = 0;

  [SerializeField] 
  private float _baseMagicResist = 0;

	[SerializeField]
	private float _baseattackdamage=0;
	
	[SerializeField]
	private float _baseattackspeed=0;

	[SerializeField]
	private float _baseattackrange=0;	


  	private float _currenthealth;
  	private float _currentresource;
	private float _Armor;
	private float _attackspeed;
	private float _attackrange;



  public void AppendHealthChange(HealthChange _change)
  {
    if (_change is Damage)
    {
			_currenthealth -= _change.value;
    } 

    if(_change is Heal)
    {
			_currenthealth += _change.value;
    }
  }
	public void AppendResourceChange(ResourceChange _change)
	{
		if (_change is Damage)
		{
			_currentresource -= _change.value;
		} 
		
		if(_change is Heal)
		{
			_currentresource += _change.value;
		}
	}
	
	
	
}
