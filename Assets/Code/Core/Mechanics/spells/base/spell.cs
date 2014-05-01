using UnityEngine;
using System.Collections;

// author paris stefanou

public class spell : SpellInterface {
	#region SpellInterface implementation

	private float _currentcooldowntime;
	private bool _ready;
	public spell(){
		Name="name";
		
		//GameObject Effect {get;set;}
		
		BaseCoolDownTime=0;
		
		CurrentCoolDown=0;
		
		Ready=true;
		
		Description="Description";
	}
	public void Cast ()
	{
		throw new System.NotImplementedException ();
	}
	
	public void update ()
	{
		throw new System.NotImplementedException ();
	}
	
	public string Name {get;set;}

	public GameObject Effect {get;set;}
	
	public float BaseCoolDownTime {get;set;}
	
	public float CurrentCoolDown {
		get {
			return(_currentcooldowntime);		}
		private set {
			_currentcooldowntime=value;
		}
	}
	public bool Ready {
		get {
			return(_ready);
		}
		private set{
			_ready=value;
			
		}	
	}
	public string Description {get;set;}
	
	#endregion




}
