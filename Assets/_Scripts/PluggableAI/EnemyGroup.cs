using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup {

	public int enemyId;
	public bool alive;
	public int maxhp;
	private int currentHp;

	public HurtableEnemyScript bot;
	public HurtableEnemyScript top;

	[HideInInspector] public Transform nTransform;
	[HideInInspector] public Transform sTransform;
	[HideInInspector] public Transform deadEnemy;
	[HideInInspector] public AStateController nStateController;
	[HideInInspector] public SStateController sStateController;
	[HideInInspector] public AttackScript nAttackScript;
	[HideInInspector] public AttackScript sAttackScript;

	private List<Transform> deadEnemies;


	public EnemyGroup(int id, int hp){
		enemyId = id;
		maxhp = hp;
		currentHp = hp;
		alive = true;
	}

	public void TakeDamage(int damage) {
		currentHp -= damage;

		if (currentHp <= 0) {
			if (bot != null)
				bot.Die();
			if (top != null)
				top.Die();
			currentHp = maxhp;
			alive = false;
		}
	}

	public void AddDead(Transform t){
		deadEnemies.Add(t);
	}

}
