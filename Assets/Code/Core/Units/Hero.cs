using UnityEngine;
using System.Collections;

// author paris stefanou

public class Hero : Unit {
	GameObject model{get;set;}
	float health{get;set;}
	float resource{get;set;}
	float armor{get;set;}
	float magicresist{get;set;}
	float attack{get;set;}
	float attackspeed{get;set;}
	float speed{get;set;}
	float cooldownreduction{get;set;}
	float criticalstrikechance{get;set;}
	float lifesteal{get;set;}
	float spellvamp{get;set;}
	int resourcetype{get;set;}

	void Start () {
		health=GV.bla_basehealth;
		resource=GV.bla_baseresource;
		armor=GV.bla_basearmor;
		magicresist=GV.bla_basemagicresist;
		attack=GV.bla_baseattack;
		attackspeed=GV.bla_baseattackspeed;
		speed=GV.bla_basespeed;
		cooldownreduction=0;
		criticalstrikechance=0;
		lifesteal=0;
		spellvamp=0;
		resourcetype=GV.bla_resourcetype;
		movemetupdate(speed,0);
	}
	void levelup () {
		health=health+GV.bla_healthscale;
		resource=resource+GV.bla_resourcescale;
		armor=armor+GV.bla_armorscale;
		magicresist=magicresist+GV.bla_magicresistscale;
		attack=attack+GV.bla_attackscale;
		attackspeed=attackspeed+GV.bla_attackspeedscale;
	}

}
