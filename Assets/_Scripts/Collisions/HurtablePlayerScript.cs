using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collider for the players which takes damage from enemy projectiles.
/// </summary>
public class HurtablePlayerScript : HurtableBaseScript {

	[Header("Player Stats")]
	public IntVariable playerMaxHealth;
	public FloatVariable damageTaken;
	public FloatVariable otherDamageTaken;
	public IntVariable playerDefense;

	[Header("Hurt stuff")]
	public BoolVariable invinciblePlayer;
	public bool canBeHurt = true;

	[Header("Events")]
	public UnityEvent playerDiedEvent;
	public UnityEvent takenDamageEvent;


	/// <summary>
	/// When hit by an enemy projectile, take damage and remove it.
	/// </summary>
	/// <param name="otherCollider"></param>
	void OnTriggerEnter2D(Collider2D otherCollider){
		Projectile projectile = otherCollider.gameObject.GetComponent<Projectile>();
		if (projectile == null) {
			return;
		}

		if (!projectile.isEnemy)
			return;

		if (!projectile.AddHitID(0))
			return;

		if (!canBeHurt || invinciblePlayer.value)
			return;

		defense = playerDefense.value;
		int dmg = TakeDamage(projectile.damage);
		damageTaken.value += dmg;
		if (dmg > 0) {
			takenDamageEvent.Invoke();
			if (projectile.impactSound != null) {
				currentSfx.value.Enqueue(projectile.impactSound.clip);
			}
			if (Random.Range(0f,1f) < hurtNoiseChance.value){
				currentSfx.value.Enqueue(hurtNoises.RandomClip());
			}
			playSfxEvent.Invoke();
		}

		if (damageTaken.value + otherDamageTaken.value >= playerMaxHealth.value)
			Die();
	}

	public override void Die() {
		base.Die();
		playerDiedEvent.Invoke();
	}
}
