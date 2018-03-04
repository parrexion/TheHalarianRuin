using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Projectile effect which is only the graphical component.
/// Used for after effects.
/// </summary>
public class Projectile : Effect {

	public bool isEnemy = false;

	[Header("Activation")]
	public float activateDelay = 0f;
	public float activeDuration = 0f;
	public bool destroyOnHit = false;
	private int activeState = 0;
	private float deactivateTime;

    [HideInInspector] public int damage = 0;
	[HideInInspector] public bool multiHit = true;
	private List<int> hitEnemies = new List<int>();
	private Collider2D coll2D;


	void Start () {
		coll2D = GetComponent<Collider2D>();
		coll2D.enabled = (activateDelay == 0);
		activeState = (activateDelay == 0) ? 1 : 0;
		deactivateTime = activateDelay + activeDuration;
	}

    protected override void Update() {
        base.Update();

		if (activeState > 1) {
			return;
		}

        if (activeState == 1) {
			if (currentTime > deactivateTime) {
				coll2D.enabled = false;
				activeState = 2;
				return;
			}
		}
		else {
			if (currentTime >= activateDelay) {
				currentTime = 0f;
				activeState = 1;
				coll2D.enabled = true;
			}
		}
    }

	
	/// <summary>
	/// Set the damage for the projectile.
	/// </summary>
	/// <param name="attackValue"></param>
	public void SetDamage(int baseDamage, int attackValue, float damageScale){
		// Debug.Log("Added some damage: " + (int)(damageScale*attackValue));
		damage = baseDamage + (int)(damageScale*attackValue);
	}

	/// <summary>
	/// Adds the id to the list of hit entities to avoid double hits.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public bool AddHitID(int id) {
		if (hitEnemies.Contains(id) && activeState != 1)
			return false;

		hitEnemies.Add(id);
		if (!multiHit) {
			activeState = 2;
			if (destroyOnHit)
				Destroy(gameObject);
		}
			
		return true;
	}
}
