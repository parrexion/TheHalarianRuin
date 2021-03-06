﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoldierGridController : MonoBehaviour {

	const float DELAY_PLAYER_HURT = 0.5f;

	[Header("Game speed")]
	public BoolVariable paused;
	public BoolVariable gridUseable;
	public BoolVariable useSlowTime;
	public BoolVariable slowSoldierSide;
	public FloatVariable slowAmount;

	[Header("References")]
	public SoldierGrid grid;
	public BattleController battleController;
	public BalanceController balanceController;

	[Header("Jumping")]
	public MoveJumpingScript moveJumping;
	public float jumpForce = 10f;

	[Header("Attack/Defend")]
	public IntVariable soldierAttack;
	public Transform starProjectile;
	public Transform lightningProjectile;
	public float AttackTimeLimit = 2.0f;
	private float currentAttackTimeLimit;

	public Transform barrier;
	private float activeBlockTime;
	private float blockTime;
	private float currentBlockTime;

	[Header("Animations")]
	public AnimationScript animScript;
	private AnimationInformation animInfo;
	private float attacking = 0;
	public HurtablePlayerScript hurtScript;
	private float hurting = 0;

	[Header("Sounds")]
	public SfxEntry basicActivationSound;
	public SfxEntry specialActivationSound;
	public AudioQueueVariable currentSfx;
	public UnityEvent playSfxEvent;



	// Use this for initialization
	void Start () {
		animInfo = new AnimationInformation();
		activeBlockTime = 0f;
		blockTime = 0f;
		gridUseable.value = true;
	}

	void Update () {
		if (paused.value)
			return;
		
		float time = (useSlowTime.value && slowSoldierSide.value) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;
		UpdateInput(time);
		UpdateAnimation(time);
	}

	/// <summary>
	/// Takes the input and updates the grid from that.
	/// </summary>
	/// <param name="timeStep"></param>
	void UpdateInput(float timeStep) {
		if (!gridUseable.value)
			return;

		if (attacking > 0.05f)
			return;

		if (blockTime != 0) {
			currentBlockTime += timeStep;
			if (currentBlockTime >= activeBlockTime) {
				hurtScript.canBeHurt = true;
			}
			if (currentBlockTime >= blockTime)
				blockTime = 0;
			return;
		}

		if (grid.attackDirection != Constants.Direction.NEUTRAL) {
			currentAttackTimeLimit += timeStep;
			if (currentAttackTimeLimit >= AttackTimeLimit) {
				grid.CancelGrid();
				return;
			}
		}
		else {
			currentAttackTimeLimit = 0;
		}

		int endReached = 0;
		if (Input.GetKeyDown(KeyCode.Space)) {
			if (moveJumping.grounded) {
				moveJumping.SetSpeed(new Vector2(0f,jumpForce), 0);
				grid.CancelGrid();
			}
		}
		else if (Input.GetKeyDown(KeyCode.W)) {

			if (grid.MoveGrid(Constants.Direction.UP))
				endReached = 1;
		}
		else if (Input.GetKeyDown(KeyCode.S)) {
			if (grid.attackDirection == Constants.Direction.NEUTRAL)
				endReached = 3;
			else if (grid.MoveGrid(Constants.Direction.DOWN))
				endReached = 1;
			else {
				endReached = 3;
				grid.CancelGrid();
			}
		}
		else if (Input.GetKeyDown(KeyCode.D)) {
			if (grid.attackDirection == Constants.Direction.LEFT) {
				endReached = 3;
				grid.CancelGrid();
			}
			else if (moveJumping.grounded) {
				if (battleController.enemyController.CheckIfEnemiesAtSide(false)) {
					currentAttackTimeLimit = 0;
					if (grid.MoveGrid(Constants.Direction.RIGHT))
						endReached = 2;
					else
						endReached = 1;
				}
				else if (grid.attackDirection == Constants.Direction.NEUTRAL) {
					//endReached = 3;
				}
				else {
					grid.CancelGrid();
				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.A)) {
			if (grid.attackDirection == Constants.Direction.RIGHT) {
				endReached = 3;
				grid.CancelGrid();
			}
			else if (moveJumping.grounded) {
				if (battleController.enemyController.CheckIfEnemiesAtSide(true)) {
					currentAttackTimeLimit = 0;
					if (grid.MoveGrid(Constants.Direction.LEFT))
						endReached = 2;
					else
						endReached = 1;
				}
				else if (grid.attackDirection == Constants.Direction.NEUTRAL) {
					//endReached = 3;
				}
				else {
					grid.CancelGrid();
				}
			}
		}

		if (endReached == 1) {
			Attack();
			attacking = 0.2f;
		}
		else if (endReached == 2) {
			EndAttack();
			attacking = 0.5f;
		}
		else if (endReached == 3) {
			Block();
		}

	}

	/// <summary>
	/// Updates the animation information for the soldier.
	/// </summary>
	/// <param name="timeStep"></param>
	public void UpdateAnimation(float timeStep) {

		animInfo.blocking = (blockTime != 0);
		animInfo.jumping = !moveJumping.grounded;

		if (hurting > 0) {
			hurting -= timeStep;
			animInfo.hurt = true;
		}
		else
			animInfo.hurt = false;
		
		animInfo.overheat = !gridUseable.value;

		if (attacking > 0) {
			animInfo.attacking = true;
			attacking -= timeStep;

			if (grid.attackDirection == Constants.Direction.LEFT)
				animInfo.mouseDirection = -1;
			else if (grid.attackDirection == Constants.Direction.RIGHT)
				animInfo.mouseDirection = 1;
			else {
				animInfo.mouseDirection = 0;
			}
		}
		else {
			animInfo.attacking = false;
			animInfo.mouseDirection = 0;
		}

		float speed = (useSlowTime.value && slowSoldierSide.value) ? slowAmount.value : 1f;
		animScript.UpdateState(animInfo, speed);
	}


	//Creates a basic attack at the given enemy
	public void Attack(){
		List<DamageValues> dmgs = new List<DamageValues>();
		if (grid.attackDirection == Constants.Direction.LEFT) {
			dmgs = battleController.enemyController.GetRandomEnemies(1,5,true,true);
		}
		else if (grid.attackDirection == Constants.Direction.RIGHT) {
			dmgs = battleController.enemyController.GetRandomEnemies(1,5,true,false);
		}

		foreach (DamageValues dv in dmgs) {
			
			if (dmgs == null)
				continue;

			Transform shotTransform = Instantiate(starProjectile) as Transform;
			Projectile projectile = shotTransform.GetComponent<Projectile>();
			projectile.SetDamage(dv.GetDamage(),soldierAttack.value,1);
			projectile.multiHit = true;
			shotTransform.position = dv.entityHit.position + new Vector3(Random.Range(-0.25f,0.25f),Random.Range(-0.25f,0.25f),0);
			balanceController.TriggerNormal();
		}
		if (dmgs.Count > 0){
			currentSfx.value.Enqueue(basicActivationSound.clip);
			playSfxEvent.Invoke();
		}
	}

	//Creates a stronger attack when the end of a branch is reached.
	public void EndAttack(){
		List<DamageValues> dmgs = new List<DamageValues>();
		SoldierEndAttack attack = balanceController.GetEndAttack();
		if (grid.lastDirection == Constants.Direction.LEFT) {
			dmgs = battleController.enemyController.GetRandomEnemies(attack.hits,attack.value,true,true);
		}
		else if (grid.lastDirection == Constants.Direction.RIGHT) {
			dmgs = battleController.enemyController.GetRandomEnemies(attack.hits,attack.value,true,false);
		}

		foreach (DamageValues dv in dmgs) {
			Transform enemy = dv.entityHit;

			var shotTransform = Instantiate(lightningProjectile) as Transform;
			shotTransform.GetComponent<Projectile>().SetDamage(dv.GetDamage(),soldierAttack.value,1);
			shotTransform.position = enemy.position;
		}
		if (dmgs.Count > 0){
			currentSfx.value.Enqueue(specialActivationSound.clip);
			playSfxEvent.Invoke();
		}
	}

	//Blocks all attacks for a moment
	public void Block(){
		if (!moveJumping.grounded)
			return;
		var barrierTransform = Instantiate(barrier) as Transform;
		barrierTransform.SetParent(transform.parent);
		barrierTransform.localPosition = transform.localPosition;
		blockTime = 1.2f;
		activeBlockTime = 0.75f;
		currentBlockTime = 0;
		hurtScript.canBeHurt = false;
	}

	/// <summary>
	/// Called when the soldier takes damage and shows the hurting animation.
	/// </summary>
	public void DamageTaken(float damageTime) {
		if (hurting <= 0)
			hurting = damageTime;
	}
}
