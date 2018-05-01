using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutsidePlayerController : MonoBehaviour {

	public BoolVariable paused;
	private MoveHomingNoLimit moveToPosition;
	public FloatVariable posx, posy;
	public FloatVariable speedHack;

	[HideInInspector] public Camera cam;

	[Header("Animations")]
	public RuntimeAnimatorController androidAnimator;
	public RuntimeAnimatorController soldierAnimator;
	private AnimationScript animScript;
	private AnimationInformation animInfo;

	[Header("Follower")]
	public BoolVariable playingAsAndroid;


	// Use this for initialization
	void Start () {
		moveToPosition = GetComponent<MoveHomingNoLimit>();
		SetPlayerPosition();
#if UNITY_EDITOR
		moveToPosition.speed *= speedHack.value;
#endif

		animScript = GetComponent<AnimationScript>();
		animInfo = new AnimationInformation();
		SetupCharacterAnimations();

		paused.value = false;
	}
	
	// Update is called once per frame
	void Update () {

		UpdateAnimation();

		if (paused.value)
			return;

		if (Input.GetMouseButton(1) && cam != null) {
			moveToPosition.moveToPosition = cam.ScreenToWorldPoint(Input.mousePosition);
		}
		posx.value = transform.position.x;
		posy.value = transform.position.y;
	}

	void SetupCharacterAnimations() {
		GetComponent<Animator>().runtimeAnimatorController = (playingAsAndroid.value) ? androidAnimator : soldierAnimator;
	}

	void SetPlayerPosition() {
		transform.position = new Vector3(posx.value,posy.value,0);
		moveToPosition.moveToPosition = transform.position;
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
