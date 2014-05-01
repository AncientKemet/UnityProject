using UnityEngine;
using System.Collections;

// author paris stefanou

public class MeeleMinion : Unit,Combatunit {

	[SerializeField]
	private float health;

	[SerializeField]
	private float armor;

	[SerializeField]
	private float magicresist;

	[SerializeField]
	private attack;

	[SerializeField]
	private float attackspeed;

	[SerializeField]
	private float speed;


	void Start () {
		health=GV.MM_basehealth+(GV.timeinminutes*GV.MM_healthscale);
		armor=GV.MM_basearmor+(GV.timeinminutes*GV.MM_armorscale);
		magicresist=GV.MM_basemagicresist+(GV.timeinminutes*GV.MM_magicresistscale);
		attack=GV.MM_baseattack+(GV.timeinminutes*GV.MM_attackscale);
		attackspeed=GV.MM_baseattackspeed+(GV.timeinminutes*GV.MM_attackspeedscale);
		speed=GV.MM_basespeed;
	}



}
