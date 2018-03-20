using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FollowerController : MonoBehaviour {

	public BoolVariable paused;
	public FloatVariable posx, posy;
	private MoveHomingNoLimit moveToPosition;
	public float minimumDistance;

	[Header("Animations")]
	public RuntimeAnimatorController androidAnimator;
	public RuntimeAnimatorController soldierAnimator;
	private AnimationScript animScript;
	private AnimationInformation animInfo;

	[Header("Follower")]
	public BoolVariable playingAsAndroid;
	public BoolVariable useFollower;


	// Use this for initialization
	void Start () {
		moveToPosition = GetComponent<MoveHomingNoLimit>();
		moveToPosition.minimumDistance = minimumDistance;
		SetPlayerPosition();

		animScript = GetComponent<AnimationScript>();
		animInfo = new AnimationInformation();
		SetupCharacterAnimations();

		paused.value = false;
		gameObject.SetActive(useFollower.value);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateAnimation();
	}

	void SetupCharacterAnimations() {
		GetComponent<Animator>().runtimeAnimatorController = (playingAsAndroid.value) ? soldierAnimator : androidAnimator;
	}

	/// <summary>
	/// Sets the start position of the follower with a small offset from the main character.
	/// </summary>
	void SetPlayerPosition() {
		float x = Random.Range(-0.5f, 0.5f);
		x = 0.5f * Mathf.Sign(x) + x;
		float y = Random.Range(-0.5f, 0.5f);
		y = 0.5f * Mathf.Sign(y) + y;
		Vector3 startOffset = minimumDistance * new Vector3(x,y,0);
		transform.position = new Vector3(posx.value,posy.value,1) + startOffset;
	}

	/// <summary>
	/// Updates the current information from the controller's state and sends it to the animator.
	/// </summary>
	public void UpdateAnimation() {
		animInfo.walkDirection = Constants.Direction.NEUTRAL;

		if (moveToPosition.moveDirectionX == -2)
			animInfo.walkDirection = Constants.Direction.LEFT;
		else if (moveToPosition.moveDirectionX == 2)
			animInfo.walkDirection = Constants.Direction.RIGHT;
		else if (moveToPosition.moveDirectionY == 2)
			animInfo.walkDirection = Constants.Direction.UP;
		else if (moveToPosition.moveDirectionY == -2)
			animInfo.walkDirection = Constants.Direction.DOWN;

		animScript.UpdateState(animInfo, (paused.value) ? 0f : 1f);
	}
}
