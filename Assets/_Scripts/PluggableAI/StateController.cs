using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StateController : BasicStateMachine {

	[Header("Enemy Values")]
	//Most of the stats
	public EnemyEntry values;
	public int enemyid;

	[Space(10)]

	[Header("Game speed")]
	public BoolVariable useSlowTime;
	public BoolVariable slowSoldierSide;
	public FloatVariable slowAmount;

	[Header("Transforms")]
	[HideInInspector] public Transform aPlayer;
	[HideInInspector] public Transform sPlayer;
	public Vector2 startPosition;

	//Attacking
	[HideInInspector] public AttackScript attack;

	[Space(5)]

	[Header("Sounds")]
	public AudioQueueVariable currentSfx;
	public UnityEvent playSfxEvent;
	

	/// /////////////////////////////////////////////////////

	protected virtual void Start() {
		if (values == null) {
			Debug.LogError("No enemy values could be found");
		}

		GameObject go = GameObject.Find("Player Android");
		if (go != null)
			aPlayer = go.transform;
		go = GameObject.Find("Player Soldier");
		if (go != null)
			sPlayer = go.transform;

		attack = GetComponent<AttackScript>();

		animInfo = new AnimationInformation();

		waitTime = 0;
		firstTime = true;
		startPosition = thisTransform.position;
		currentWaitState = WaitStates.MOVE;
		waitTimeLimits = values.waitTimeLimits;
		moveSpeed = values.speed.x;
	}

	protected override void Update () {
		if (paused.value)
			return;

		firstTime = (stateTimeElapsed == 0);
		stateTimeElapsed += (useSlowTime.value && !slowSoldierSide.value) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;
		currentState.UpdateState(this);
		UpdateAnimation();
	}

	private void OnDrawGizmos() {
		if (currentState != null) {
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere(transform.position,1.0f);
		}
	}

	public override WaitStates GetRandomWaitState(){
		int select = Random.Range(0,values.waitStates.Count);
		return values.waitStates[select];
	}

	public abstract Vector3 GetRandomLocation();

}
