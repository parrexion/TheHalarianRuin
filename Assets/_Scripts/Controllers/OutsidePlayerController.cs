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
	public AnimationScript animScript;
	private AnimationInformation animInfo;

	[Header("Follower")]
	public BoolVariable useFollower;
	public GameObject follower;


	// Use this for initialization
	void Start () {
		moveToPosition = GetComponent<MoveHomingNoLimit>();
		animInfo = new AnimationInformation();
		SetPlayerPosition();
		paused.value = false;
#if UNITY_EDITOR
		moveToPosition.speed *= speedHack.value;
#endif
		follower.gameObject.SetActive(useFollower.value);
	}
	
	// Update is called once per frame
	void Update () {

		if (paused.value)
			return;

		if (Input.GetMouseButton(1) && cam != null) {
			moveToPosition.moveToPosition = cam.ScreenToWorldPoint(Input.mousePosition);
		}
		posx.value = transform.position.x;
		posy.value = transform.position.y;

		UpdateAnimation(Time.deltaTime);
	}


	public void SetPlayerPosition() {
		transform.position = new Vector3(posx.value,posy.value,0);
		moveToPosition.moveToPosition = transform.position;
		Debug.Log("Position is now: " + posx.value + ", " + posy.value);
	}

	/// <summary>
	/// Updates the current information from the controller's state and sends it to the animator.
	/// </summary>
	public void UpdateAnimation(float timeStep) {
		animInfo.walkDirection = Constants.Direction.NEUTRAL;

		if (moveToPosition.moveDirectionX == 1)
			animInfo.walkDirection = Constants.Direction.LEFT;
		else if (moveToPosition.moveDirectionX == -1)
			animInfo.walkDirection = Constants.Direction.RIGHT;

		animScript.UpdateState(animInfo, 1f);
	}
}
