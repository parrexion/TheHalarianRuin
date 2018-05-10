using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Projectile effect which is only the graphical component.
/// Used for after effects.
/// </summary>
public class Projectile : Effect {

	[Space(10)]
	public bool isEnemy = false;
    public int damage = 0;
    private int tempDamage = 0;
	[HideInInspector] public bool multiHit = true;
	private List<int> hitEnemies = new List<int>();
	private Collider2D coll2D;


	protected override void AdditionalSetup() {
		coll2D = GetComponent<Collider2D>();
	}

	protected override void AdditionalCurrentStepSetup() {
		EffectStep step = steps[currentStep];
		damage = (step.damage) ? tempDamage : 0;
		coll2D.enabled = (damage != 0);
		transform.Translate(new Vector3(0,0,0.001f));
		hitEnemies.Clear();
	}
	
	/// <summary>
	/// Set the damage for the projectile.
	/// </summary>
	/// <param name="attackValue"></param>
	public void SetDamage(int baseDamage, int attackValue, float damageScale){
		tempDamage = baseDamage + (int)(damageScale*attackValue);
	}

	/// <summary>
	/// Adds the id to the list of hit entities to avoid double hits.
	/// </summary>
	/// <param name="id"></param>
	/// <returns></returns>
	public bool AddHitID(int id) {
		if (hitEnemies.Contains(id) || damage == 0)
			return false;

		hitEnemies.Add(id);
		if (!multiHit) {
			TriggerNextStep();
		}
			
		return true;
	}
}
