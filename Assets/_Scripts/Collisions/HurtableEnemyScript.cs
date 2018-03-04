using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Collider script for enemies which reacts to player projectiles.
/// </summary>
[RequireComponent(typeof(StateController),typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer),typeof(Rigidbody2D))]
public class HurtableEnemyScript : MonoBehaviour {

	private static int id = 0;

	public EnemyGroup group;
	public Transform damageNumbers;
	public BoolVariable invincibleEnemy;

	private BattleGUIController battleGUI;
	private SpriteRenderer spriteRenderer;
	private StateController stateController;
	private Rigidbody2D rigid;
	private Collider2D collider2d;


	void Start(){
		battleGUI = MainControllerScript.instance.battleGUI;
		spriteRenderer = GetComponent<SpriteRenderer>();
		stateController = GetComponent<StateController>();
		rigid = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
	}

	/// <summary>
	/// When colliding with player projectiles, take damage if it hasn't been hit by it yet.
	/// </summary>
	/// <param name="otherCollider"></param>
	void OnTriggerEnter2D(Collider2D otherCollider){

		Projectile projectile = otherCollider.gameObject.GetComponent<Projectile>();
		if (projectile == null) {
			Debug.Log("Null");
			return;
		}
		if (projectile.isEnemy)
			return;

		if (!projectile.AddHitID(group.enemyId))
			return;

		if (invincibleEnemy.value)
			return;

		group.TakeDamage(projectile.damage);

		Transform t = Instantiate(damageNumbers);
		t.position = transform.position;
		DamageNumberDisplay dnd = t.GetComponent<DamageNumberDisplay>();
		dnd.damage = projectile.damage;
		battleGUI.damages.Add(dnd);
		id++;
	}

	/// <summary>
	/// Removes the dead enemy and spawns a dead version of it.
	/// </summary>
	public void Die(){
		spriteRenderer.enabled = false;
		stateController.enabled = false;
		rigid.Sleep();
		collider2d.enabled = false;
		
		Transform t = Instantiate(group.deadEnemy);
		t.position = transform.position;
		t.localScale = transform.localScale;
		Destroy(gameObject,1/t.GetComponent<FadeAwayScript>().fadeDuration);
	}
}
