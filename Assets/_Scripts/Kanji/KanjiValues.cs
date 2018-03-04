using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KanjiValues {

	public enum KanjiType {
		CLICK,SLASH,RISE,HOLD,DOWN,OTHER
	}

	[Header("Usage values")]
	public float startCooldownTime = 0f;
	public int maxCharges = 10;
	public float delay = 0.1f;
	public float cooldown = 5.0f;

	[Space(10)]

	[Header("Activation requirements")]
	public KanjiType kanjiType;
	public float area = 1.0f; 
	public float holdMin = -1f;
	public float holdMax = 0.5f;

	[Space(10)]

	[Header("Projectile")]
	public Transform projectile;
	public Vector2 projectileSpeed;
	public float projectileLifetime = 1f;

	[Space(10)]

	[Header("Damage")]
	public int damage = 0;
	public float baseDamageScale = 1.0f;
	public bool multihit = true;


	public void ResetValues() {
		startCooldownTime = 0;
		maxCharges = 10;
		delay = 0.1f;
		cooldown = 5f;

		kanjiType = KanjiType.OTHER;
		area = 1.0f;
		holdMin = -1f;
		holdMax = 0.5f;

		projectile = null;
		projectileSpeed = Vector2.zero;
		projectileLifetime = 1f;

		damage = 0;
		baseDamageScale = 1f;
		multihit = true;
	}

	public void CopyValues(KanjiValues other) {
		startCooldownTime = other.startCooldownTime;
		maxCharges = other.maxCharges;
		delay = other.delay;
		cooldown = other.cooldown;

		kanjiType = other.kanjiType;
		area = other.area;
		holdMin = other.holdMin;
		holdMax = other.holdMax;

		projectile = other.projectile;
		projectileSpeed = other.projectileSpeed;
		projectileLifetime = other.projectileLifetime;

		damage = other.damage;
		baseDamageScale = other.baseDamageScale;
		multihit = other.multihit;
	}
}
