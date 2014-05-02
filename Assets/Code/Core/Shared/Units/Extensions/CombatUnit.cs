using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class CombatUnit : MoveableUnit
{

  List<CombatStatBuff> modifiers = new List<CombatStatBuff>();

  [SerializeField]
  private float
    _baseHealth = 100f;
  [SerializeField]
  private float
    _baseresource = 100f;
  [SerializeField] 
  private float
    _baseArmor = 0;
  [SerializeField] 
  private float
    _baseMagicResist = 0;
  [SerializeField]
  private float
    _baseattackdamage = 0;
  [SerializeField]
  private float
    _baseattackspeed = 0;
  [SerializeField]
  private float
    _baseattackrange = 0;
  private float _currenthealth;
  private float _currentresource;

  public float CurrentArmor
  {
    get
    {
      float _calculatedValue = _baseArmor;

      foreach (CombatStatBuff _bonusFlatArmorBuff in GetAllBuffsOfType(CombatStatBuff.TYPE.FLAT_ARMOR))
      {
        _calculatedValue += _bonusFlatArmorBuff.value;
      }

      foreach (CombatStatBuff _buff in GetAllBuffsOfType(CombatStatBuff.TYPE.BONUS_FLAT_ARMOR_OVER_TIME))
      {
        _calculatedValue += _buff.value * (( Time.time - _buff.startTime) / (_buff.duration));
      }

      foreach (CombatStatBuff _modifier in GetAllBuffsOfType(CombatStatBuff.TYPE.PERCENTAGE_ARMOR))
      {
        _calculatedValue *= _modifier.value;
      }
      
      return _calculatedValue;
    }
  }

  /// <summary>
  /// Gets the current attack speed.
  /// </summary>
  /// <value>The current attack speed.</value>
  public float CurrentAttackSpeed
  {
    get
    {
      float _calculatedValue = _baseattackspeed;

      foreach (CombatStatBuff _modifier in GetAllBuffsOfType(CombatStatBuff.TYPE.ATTACK_SPEED))
      {
        _calculatedValue *= _modifier.value;
      }

<<<<<<< HEAD
  	private float _currenthealth;
  	private float _currentresource;
	private float _Armor;
	private float _attackspeed;
	private float _attackrange;


=======
      return _calculatedValue;
    }
  }
>>>>>>> d4b99cc5d6f2048a1b6b3525e41fb7a291ac34d9

  public void AddBuff(CombatStatBuff buff)
  {
    modifiers.Add(buff);
    buff.startTime = Time.time;
    StartCoroutine(StartRemovingBuff(buff, this));
  }

  /// <summary>
  /// Appends the health change.
  /// </summary>
  /// <param name="_change">_change.</param>
  public void AppendHealthChange(HealthChange _change)
  {
    if (_change is Damage)
    {
      _currenthealth -= _change.value;
    } 
    
    if (_change is Heal)
    {
      _currenthealth += _change.value;
    }
  }

  /// <summary>
  /// Gets the type of the all buffs of.
  /// </summary>
  /// <returns>The all buffs of type.</returns>
  /// <param name="type">Type.</param>
  public List<CombatStatBuff> GetAllBuffsOfType(CombatStatBuff.TYPE type)
  {
    List<CombatStatBuff> list = new List<CombatStatBuff>();
    foreach (var item in modifiers)
    {
      if (item.type == type)
      {
        list.Add(item);
      }
    }
    return list;
  }

  IEnumerator StartRemovingBuff(CombatStatBuff buff, CombatUnit combatUnit)
  {
    yield return new WaitForSeconds(buff.duration);
    if (combatUnit != null)
    {
      combatUnit.modifiers.Remove(buff);
    }
  }
  
  public class CombatStatBuff
  {

    public enum TYPE
    {
      PERCENTAGE_ARMOR, FLAT_ARMOR, BONUS_FLAT_ARMOR_OVER_TIME,
      ATTACK_SPEED
    }

    public CombatStatBuff.TYPE type;
    public float value;
    public float duration;
    public float startTime;

  }


  
}
