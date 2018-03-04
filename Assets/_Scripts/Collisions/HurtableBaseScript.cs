using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HurtableBaseScript : MonoBehaviour {

	public BoolVariable invincible;
	protected int defense;


	public virtual int TakeDamage(int damage) {

		if (invincible.value){
			return 0;
		}

		damage -= defense;

		return damage;
	}
	
	public virtual int Heal(int currentDamage, int heal){
		int oldDamage = currentDamage;
		currentDamage = Mathf.Max(currentDamage - heal,0);
		Debug.Log("Healed "+(oldDamage - currentDamage)+" damage");

		return currentDamage;
	}

	public virtual void Die(){
		//Implement for what should happen when
		//the character dies.
		Debug.Log("The character died!");
	}
}
