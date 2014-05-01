using UnityEngine;
using System.Collections;

// author paris stefanou

public class MeeleMinion : Unit {
	GameObject model{get;set;}
	float health{get;set;}
	float armor{get;set;}
	float magicresist{get;set;}
	float attack{get;set;}
	float attackspeed{get;set;}
	float speed{get;set;}


	void Start () {
		health=GV.MM_basehealth+(GV.timeinminutes*GV.MM_healthscale);
		armor=GV.MM_basearmor+(GV.timeinminutes*GV.MM_armorscale);
		magicresist=GV.MM_basemagicresist+(GV.timeinminutes*GV.MM_magicresistscale);
		attack=GV.MM_baseattack+(GV.timeinminutes*GV.MM_attackscale);
		attackspeed=GV.MM_baseattackspeed+(GV.timeinminutes*GV.MM_attackspeedscale);
		speed=GV.MM_basespeed;
	}



}
