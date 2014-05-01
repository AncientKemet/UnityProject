using UnityEngine;
using System.Collections;

// author paris stefanou

public interface SpellInterface  {

	string Name{get;set;}

	GameObject Effect {get;set;}

	float BaseCoolDownTime{get;set;}

	float CurrentCoolDown{get;}

	bool Ready{get;}

	string Description{get;set;}
	void Cast();
	void update();

}
