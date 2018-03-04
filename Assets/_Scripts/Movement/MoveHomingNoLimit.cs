using UnityEngine;

/// <summary>
/// Move script which moves towards a certain position or object without any boundries.
/// </summary>
public class MoveHomingNoLimit : MonoBehaviour {

	public BoolVariable paused;
	public Vector2 speed = new Vector2(5, 5);
	public Vector2 moveToPosition = new Vector2(0, 0);
	public Transform objectToFollow;

	public Vector2 movement;
	private Rigidbody2D rigidbodyComponent;


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
			movement = Vector2.MoveTowards(transform.position,moveToPosition,speed.x*Time.fixedDeltaTime);
		}
		else {
			movement = Vector2.MoveTowards (transform.position, objectToFollow.position, speed.x * Time.fixedDeltaTime);
		}

		rigidbodyComponent.MovePosition(movement);
	}

}