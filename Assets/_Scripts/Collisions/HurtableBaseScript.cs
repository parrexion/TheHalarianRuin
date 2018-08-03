using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class HurtableBaseScript : MonoBehaviour {

	public BoolVariable invincible;
	protected int defense;

	[Header("Sounds")]
	public AudioQueueVariable currentSfx;
	public FloatVariable hurtNoiseChance;
	public SfxList hurtNoises;
	public UnityEvent playSfxEvent;


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
