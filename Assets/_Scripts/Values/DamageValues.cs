using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageValues {

	public int baseDamage;
	public Transform entityHit;
	private float resistance;

	public DamageValues(Transform entity){
		baseDamage = 0;
		resistance = 1;
		entityHit = entity;
	}

	public int GetDamage(){
//		Debug.Log("Damage:  "+(int)(baseDamage*resistance));
		return (int)(baseDamage*resistance);
	}
}
