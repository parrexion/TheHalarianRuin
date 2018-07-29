using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StateController : MonoBehaviour {

	public enum WaitStates {MOVE,CHASE,FLEE,RANGE}

	[Header("Enemy Values")]
	//Most of the stats
	public EnemyEntry values;
	public int enemyid;

	[Space(10)]

	[Header("Game speed")]
	public BoolVariable paused;
	public BoolVariable useSlowTime;
	public BoolVariable slowSoldierSide;
	public FloatVariable slowAmount;

	[Header("Transforms")]
	[HideInInspector] public Transform aPlayer;
	[HideInInspector] public Transform sPlayer;
	[HideInInspector] public Transform thisTransform;
	[HideInInspector] public Rigidbody2D rigidBody;
	public Vector2 startPosition;

	[Header("Animations")]
	public AnimationScript animScript;
	public AnimationInformation animInfo;

	[Space(5)]

	[Header("Sounds")]
	public AudioVariable currentSfx;
	public UnityEvent playSfxEvent;

	[Space(10)]

	[Header("AI State Machine")]
	public WaitStates currentWaitState;
	public State currentState;
	public State remainState;
	[HideInInspector] public float stateTimeElapsed = 0;
	public bool firstTime = true;
	//Waiting
	public bool finishedWaiting = false;
	public float waitTime = 0;
	//Attacking
	[HideInInspector] public AttackScript attack;
	public bool hasAttacked = false;
	//
	

	/// /////////////////////////////////////////////////////

	protected virtual void Start(){
		if (values == null) {
			Debug.LogError("No enemy values could be found");
		}

		GameObject go = GameObject.Find("Player Android");
		if (go != null)
			aPlayer = go.transform;
		go = GameObject.Find("Player Soldier");
		if (go != null)
			sPlayer = go.transform;

		thisTransform = GetComponent<Transform>();
		rigidBody = GetComponent<Rigidbody2D>();
		attack = GetComponent<AttackScript>();

		animInfo = new AnimationInformation();

		waitTime = 0;
		firstTime = true;
		startPosition = thisTransform.position;
		currentWaitState = WaitStates.MOVE;
	}

	// Update is called once per frame
	void Update () {
		if (paused.value)
			return;

		firstTime = (stateTimeElapsed == 0);
		stateTimeElapsed += (useSlowTime.value && !slowSoldierSide.value) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;
		currentState.UpdateState(this);
		UpdateAnimation();
	}

	public void TransitionToState(State nextState) {
		if (nextState != remainState) {
			currentState = nextState;
			stateTimeElapsed = 0;
			waitTime = 0;
			hasAttacked = false;
			OnExitState();
		}
	}

	protected abstract void OnExitState();

	protected abstract void UpdateAnimation();

	private void OnDrawGizmos() {
		if (currentState != null) {
			Gizmos.color = currentState.sceneGizmoColor;
			Gizmos.DrawWireSphere(transform.position,1.0f);
		}
	}

	public WaitStates GetRandomWaitState(){
		int select = Random.Range(0,values.waitStates.Count);
		return values.waitStates[select];
	}

	public abstract Vector3 GetRandomLocation();

}
