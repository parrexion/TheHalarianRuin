using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaitStates {MOVE,CHASE,FLEE,RANGE}
	
public abstract class BasicStateMachine : MonoBehaviour {

	public BoolVariable paused;

	[Header("AI State Machine")]
	public WaitStates currentWaitState;
	public State currentState;
	public State remainState;
	[HideInInspector] public float stateTimeElapsed = 0;
	public bool firstTime = true;
	public bool hasAttacked = false;
	//Waiting
	public RangedFloat waitTimeLimits = new RangedFloat(3f,5f);
	public bool finishedWaiting = false;
	public float waitTime = 0;
	//

	//Movement
	[HideInInspector] public Transform thisTransform;
	[HideInInspector] public Rigidbody2D rigidBody;
	[HideInInspector] public Vector2 movement;
	[HideInInspector] public Vector2 moveToPoint = new Vector2(-999,-999);
	public float moveSpeed;
	public BoxCollider2D moveBounds;

	[Header("Animations")]
	public AnimationScript animScript;
	public AnimationInformation animInfo;
	[HideInInspector] public int moveDirection;
	protected int lastTime = 0;


	private void Awake() {
		thisTransform = GetComponent<Transform>();
		rigidBody = GetComponent<Rigidbody2D>();
		moveToPoint = new Vector2(-999,-999);
	}

	// Update is called once per frame
	protected virtual void Update () {
		if (paused.value)
			return;

		firstTime = (stateTimeElapsed == 0);
		stateTimeElapsed += Time.deltaTime;
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

	public abstract WaitStates GetRandomWaitState();

	protected abstract void OnExitState();

	protected abstract void UpdateAnimation();

}
