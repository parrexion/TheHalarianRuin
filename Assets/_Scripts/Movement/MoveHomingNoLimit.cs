using UnityEngine;

/// <summary>
/// Move script which moves towards a certain position or object without any boundries.
/// </summary>
public class MoveHomingNoLimit : MonoBehaviour {

	public BoolVariable paused;
	public Vector2 speed = new Vector2(5, 5);
	public Vector2 moveToPosition = Vector2.zero;
	public Transform objectToFollow;
	public int moveDirectionX = 0;

	public Vector2 movement;
	private Rigidbody2D rigidbodyComponent;
	public float minimumDistance = 0f;


	void Start() {
		if (rigidbodyComponent == null) 
			rigidbodyComponent = GetComponent<Rigidbody2D>();
		moveToPosition = new Vector2(transform.position.x,transform.position.y);
	}

	/// <summary>
	/// Updates the position to fololow the object or position.
	/// </summary>
	void FixedUpdate() {
		if (paused.value)
			return;

		if (objectToFollow == null) {
			if (Vector2.Distance(transform.position, moveToPosition) > minimumDistance) {
				movement = Vector2.MoveTowards(transform.position,moveToPosition,speed.x*Time.fixedDeltaTime);
				CalculateAnimationAngle();
			}
			else {
				movement = transform.position;
				moveDirectionX = 0;
			}
		}
		else if (Vector2.Distance(transform.position, objectToFollow.position) > minimumDistance) {
			movement = Vector2.MoveTowards(transform.position, objectToFollow.position, speed.x * Time.fixedDeltaTime);
			CalculateAnimationAngle();
		}
		else {
			movement = transform.position;
			moveDirectionX = 0;
		}
		
		rigidbodyComponent.MovePosition(movement);
	}

	void CalculateAnimationAngle() {
		Debug.Log("Move: " + (movement.x - transform.position.x));
		if (movement.x < transform.position.x)
			moveDirectionX = -1;
		else
			moveDirectionX = 1;
	}

}