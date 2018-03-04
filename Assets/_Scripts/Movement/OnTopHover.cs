using UnityEngine;

/// <summary>
/// Makes the transform stay on top of the given position or object to follow.
/// </summary>
public class OnTopHover : MonoBehaviour {

	public float borderWidth = 3.5f;
	public float borderHeight = 3.5f;

	private float startX;
	private float startY;

	public Vector3 moveToPosition = new Vector3(0, 0,-10);
	public Transform objectToFollow;

	private Vector3 movement;


	void Start() {
		startX = transform.position.x;
		startY = transform.position.y;
	}

	/// <summary>
	/// Moves the transform towards to the position.
	/// </summary>
	void FixedUpdate() {

		if (objectToFollow == null)
			movement = moveToPosition;
		else
			movement = objectToFollow.position;

		movement.Set(
			Mathf.Clamp(movement.x,startX-borderWidth,startX+borderWidth),
			Mathf.Clamp(movement.y,startY-borderHeight,startY+borderHeight),
			-10);

		transform.position = movement;
	}

}