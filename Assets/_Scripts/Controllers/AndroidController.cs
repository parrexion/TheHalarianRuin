using UnityEngine;
using System.Collections;

/// <summary>
/// Class for the controller for the android player during battles which handles 
/// the input and triggers both moving and attacking using that.
/// </summary>
public class AndroidController : MonoBehaviour {

    private enum Type { ANDROID, SOLDIER };

	public Camera screenCamera;

	[Header("Game speed")]
	public BoolVariable paused;
	public BoolVariable useSlowTime;
	public BoolVariable slowSoldierSide;
	public FloatVariable slowAmount;

	private float startX;
	private float startY;

	[Header("Animations")]
	public AnimationScript animScript;
	private AnimationInformation animInfo;
	public HurtablePlayerScript hurtScript;
	private float hurting = 0;

	private MoveHomingScript moveToPosition;
	private Rigidbody2D rigidbodyComponent;
	private Collider2D coll2D;
    private MouseInformation mouseInfo;

	[Header("Attacks")]
	public WeaponSlot weapon;

	const float delayPlayerHurt = 0.5f;
	const float delayUntilCharging = 0.25f;
	const float invulFramesForDash = 0.25f;

	// Use this for initialization
	void Start () {
		if (rigidbodyComponent == null)
			rigidbodyComponent = GetComponent<Rigidbody2D>();
		if (coll2D == null)
			coll2D = GetComponent<Collider2D>();
		if (moveToPosition == null)
			moveToPosition = GetComponent<MoveHomingScript>();

		mouseInfo = new MouseInformation();
		startX = transform.position.x;
		startY = transform.position.y;
		mouseInfo.playerPosition = transform.position;
		mouseInfo.holding = false;
		mouseInfo.holdDuration = -1;
		animInfo = new AnimationInformation();
	}
	
	// Update is called once per frame
	void Update () {

		if (paused.value)
			return;

		coll2D.enabled = (moveToPosition.GetDashPercent() > invulFramesForDash);

		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x,startX-Constants.cameraBorderWidth,startX+Constants.cameraBorderWidth),
			Mathf.Clamp(transform.position.y,startY-Constants.cameraBorderHeight,startY+Constants.cameraBorderHeight),
			0);
		mouseInfo.playerPosition = transform.position;
		Vector3 pos = screenCamera.ScreenToWorldPoint(Input.mousePosition);
		mouseInfo.mousePosition = pos;
		mouseInfo.clicked = false;

		if (moveToPosition.dashing)
			return;

		float deltaTime = (useSlowTime.value && !slowSoldierSide.value) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;

		if (Input.GetMouseButtonDown(0)) {
			mouseInfo.holding = true;
			mouseInfo.holdDuration = 0;
			mouseInfo.position1 = screenCamera.ScreenToWorldPoint(Input.mousePosition);
		}

		if (mouseInfo.holding) {
			mouseInfo.setPosition2(screenCamera.ScreenToWorldPoint(Input.mousePosition));
			mouseInfo.holdDuration += deltaTime;
		}
	
		if (Input.GetMouseButtonUp(0)) {
			mouseInfo.holding = false;
			mouseInfo.setPosition2(pos);
			mouseInfo.clicked = true;
		}

		weapon.Activate(mouseInfo);

		if (Input.GetMouseButtonDown(1) && !mouseInfo.holding) {
			if (Input.GetKey(KeyCode.LeftShift)) {
				moveToPosition.moveToPosition = pos;
				moveToPosition.startDash();
			}
			else if (!moveToPosition.dashing)
				moveToPosition.moveToPosition = pos;
		}

		UpdateAnimation(deltaTime);
    }

	/// <summary>
	/// Updates the current information from the controller's state and sends it to the animator.
	/// </summary>
	public void UpdateAnimation(float timeStep) {

		if (hurting > 0) {
			hurting -= timeStep;
			animInfo.hurt = true;
		}
		else
			animInfo.hurt = false;

		animInfo.dashing = moveToPosition.dashing;

		if (weapon.IsAttacking()) {
			animInfo.attacking = true;
			animInfo.blocking = false;
			if (mouseInfo.position1.x < transform.position.x)
				animInfo.mouseDirection = -1;
			else
				animInfo.mouseDirection = 1;
		}
		else if (mouseInfo.holding && mouseInfo.holdDuration > delayUntilCharging) {
			animInfo.attacking = false;
			animInfo.blocking = true;
			if (mouseInfo.position1.x < transform.position.x)
				animInfo.mouseDirection = -1;
			else
				animInfo.mouseDirection = 1;
		}
		else {
			animInfo.attacking = false;
			animInfo.blocking = false;
			animInfo.mouseDirection = moveToPosition.moveDirection;
		}
		float speed = (useSlowTime.value && !slowSoldierSide.value) ? slowAmount.value : 1f;
		animScript.UpdateState(animInfo, speed);
	}

	public void SetHurtAnimation() {
		hurting = delayPlayerHurt;
	}
}
