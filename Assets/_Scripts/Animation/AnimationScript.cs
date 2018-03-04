using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class containing the base animation information needed for the animations.
/// </summary>
public abstract class AnimationScript : MonoBehaviour {

	protected Animator animator;
	protected State currentState;
	protected Vector3 defaultScale;

	public enum StateString {
		Idle,
		WalkLeft,
		WalkRight,
		Chase,
		Attack,
		Block,
		Hurt,
		Jump,
		Dash
	}

	//Animations
	protected const string kIdleAnim = "Idle";
	protected const string kWalkLAnim = "WalkLeft";
	protected const string kWalkRAnim = "WalkRight";
	protected const string kChaseAnim = "Chase";
	protected const string kAttackAnim = "Attack";
	protected const string kBlockAnim = "Block";
	protected const string kHurtAnim = "Hurt";
	protected const string kJumpAnim = "Jump";
	protected const string kDashAnim = "Dash";


	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		defaultScale = transform.localScale;
	}


	public abstract void UpdateState(AnimationInformation info, float speed);


	protected void Face(int direction) {
		transform.localScale = new Vector3(defaultScale.x*direction,defaultScale.y,defaultScale.z);
	}

}
