using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LibraryEntries/Enemy")]
public class EnemyEntry : ScrObjLibraryEntry {

    [Header("General")]
    public Transform enemyModelN;
    public Transform enemyModelS;
	public int maxhp = 1;
	public Vector2 speed = new Vector2(0f,0f);

	[Space(5)]

	[Header("AI Values")]
	public List<StateController.WaitStates> waitStates = new List<StateController.WaitStates>();
	[MinMaxRangeAttribute(0.1f,10.0f)]
	public RangedFloat waitTimeLimits = new RangedFloat(3f,5f);

	[Range(0.5f,20.0f)]
	public float chaseTimeLimit = 30f;

	[Range(1.0f,10.0f)]
	public float fleeDistance = 3f;
	[Range(0.5f,3.0f)]
	public float fleeTimeLimit = 30f;

	[Space(5)]

	[Header("Attacking")]
	[Range(0.5f,3.0f)]
	public float meleeRange = 1f;
	[Range(0.1f,10.0f)]
	public float attackRate = 1f;
	public int attacks = 1;
	public float meleeTimeStartup = 0.5f;
	public float meleeTimeAnimation = 0.5f;

	[Space(5)]

	[Header("Reward")]
	public int exp = 0;
	public int money = 0;
	//Add some kind of loot table


    public override void ResetValues(){
        base.ResetValues();

        // General
        enemyModelN = null;
        enemyModelS = null;
        maxhp = 1;
        speed = new Vector2(0f,0f);

        // AI values
        waitStates = new List<StateController.WaitStates>();
        waitTimeLimits = new RangedFloat(3f,5f);
        chaseTimeLimit = 30f;
        fleeDistance = 3f;
        fleeTimeLimit = 30f;

        // Attacking
        meleeRange = 1f;
        attackRate = 1f;
        attacks = 1;
        meleeTimeStartup = 0.5f;
        meleeTimeAnimation = 0.5f;

        // Reward
        exp = 0;
        money = 0;
        //Add some kind of loot table
    }

    public override void CopyValues(ScrObjLibraryEntry other) {
        base.CopyValues(other);
        EnemyEntry ee = (EnemyEntry)other;

        // General
        enemyModelN = ee.enemyModelN;
        enemyModelS = ee.enemyModelS;
        maxhp = ee.maxhp;
        speed = ee.speed;

        // AI values
        waitStates = new List<StateController.WaitStates>();
        for (int i = 0; i < ee.waitStates.Count; i++) {
            waitStates.Add(ee.waitStates[i]);
        }
        waitTimeLimits = new RangedFloat(ee.waitTimeLimits.minValue, ee.waitTimeLimits.maxValue);
        chaseTimeLimit = ee.chaseTimeLimit;
        fleeDistance = ee.fleeDistance;
        fleeTimeLimit = ee.fleeTimeLimit;

        // Attacking
        meleeRange = ee.meleeRange;
        attackRate = ee.attackRate;
        attacks = ee.attacks;
        meleeTimeStartup = ee.meleeTimeStartup;
        meleeTimeAnimation = ee.meleeTimeAnimation;

        // Reward
        exp = ee.exp;
        money = ee.money;
        //Add some kind of loot table
    }
}
