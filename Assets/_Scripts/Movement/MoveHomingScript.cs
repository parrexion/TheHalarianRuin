using UnityEngine;

/// <summary>
/// Class which moves the object towards a position or to follow an object and 
/// stay within the boundaries. Also allows for dashing
/// </summary>
public class MoveHomingScript : MonoBehaviour {

	[Header("Game speed")]
	public BoolVariable paused;
	public BoolVariable canBeSlowed;
	public BoolVariable slowLeftSide;
	public FloatVariable slowAmount;

	[Header("Movement values")]
	public Vector2 speed = new Vector2(5, 5);
	public Vector2 moveToPosition = new Vector2(0, 0);
	public Transform objectToFollow;
	public int moveDirection = 0;
	public BoxCollider2D moveBounds;

	private Vector2 movement;
	private Rigidbody2D rigidbodyComponent;

	[Header("Dash")]
	public bool dashing = false;
	public Vector2 dashSpeed = new Vector2(0.3f,0.3f);
	public float dashTime = 0.75f;
	private float currentDashTime;
	private Vector2 currentDashSpeed;


	void Start() {
		if (rigidbodyComponent == null) 
			rigidbodyComponent = GetComponent<Rigidbody2D>();
		moveToPosition = new Vector2(transform.position.x,transform.position.y);
		currentDashTime = dashTime;
	}

	/// <summary>
	/// Lets the player dash if not currently dashing.
	/// </summary>
	public void StartDash() {
		if (dashing)
			return;
		dashing = true;
		currentDashSpeed = speed*1.5f;
		currentDashTime = 0f;

		if (moveToPosition.x < transform.position.x)
			moveDirection = -1;
		else
			moveDirection = 1;

		Vector2 vvv = new Vector2((moveToPosition.x - transform.position.x),(moveToPosition.y - transform.position.y));

		vvv = vvv.normalized;
		vvv *= 10;
		moveToPosition += vvv;
	}

	/// <summary>
	/// Moves the transform towards the position or the object to follow.
	/// </summary>
	void FixedUpdate() {
		if (paused.value)
			return;

		float time = (canBeSlowed.value && !slowLeftSide.value) ? (Time.fixedDeltaTime * slowAmount.value) : Time.fixedDeltaTime;
		float slowed = (canBeSlowed.value && !slowLeftSide.value) ? slowAmount.value : 1f;

		if (objectToFollow == null) {
			if (dashing) {
				movement = Vector2.MoveTowards(transform.position,moveToPosition,currentDashSpeed.x*time);
				currentDashSpeed -= slowed * dashSpeed;
				currentDashTime += time;
				dashing = (currentDashTime < dashTime);
				if (!dashing)
					moveToPosition = transform.position;
			}
			else
				movement = Vector2.MoveTowards(transform.position,moveToPosition,speed.x*time);
			if (Vector2.Distance(transform.position,moveToPosition) > 0.35) {
				if (moveToPosition.x < transform.position.x)
					moveDirection = -1;
				else
					moveDirection = 1;
			}
			else {
				moveDirection = 0;
			}
		}
		else {
			movement = Vector2.MoveTowards (transform.position, objectToFollow.position, speed.x * time);
			if (Vector2.Distance(transform.position,objectToFollow.position) > 0.25) {
				if (objectToFollow.position.x < transform.position.x)
					moveDirection = -1;
				else
					moveDirection = 1;
			}
			else {
				moveDirection = 0;
			}
		}
		
		movement = moveBounds.bounds.ClosestPoint(movement);
		
		rigidbodyComponent.MovePosition(movement);
	}

	/// <summary>
	/// Calculates how much is left of the dash in percent.
	/// </summary>
	/// <returns></returns>
	public float GetDashPercent(){
		return currentDashTime/dashTime;
	}
}