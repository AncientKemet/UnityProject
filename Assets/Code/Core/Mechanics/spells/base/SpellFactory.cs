using UnityEngine;
using System.Collections;

// author paris stefanou

public class SpellGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//spell spell =CreateSpell ();

		//Debug.Log(spell.Name);
	}
	public spell CreateSpell(string name,GameObject effect,float basecooldowntime,string description){
		spell spell=new spell();
		spell.Name=name;
		spell.Effect=effect;
		spell.BaseCoolDownTime=basecooldowntime;
		spell.Description=description;
		return(spell);
	}
}
