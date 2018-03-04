using UnityEngine;

/// <summary>
/// Class which moves the object towards a position or to follow an object and 
/// stay within the boundaries. Also allows for dashing
/// </summary>
public class MoveJumpingScript : MoveScript {

	[Header("Movement values")]
	public bool grounded = false;
	public float groundLevel;
	public float gravityIntensity = -10f;
	public float fallMultiplier = 2.5f;

	Vector2 unscaledSpeed = new Vector2();
	bool jumping = false;


    public override void setSpeed(Vector2 baseSpeed, float rotation) {

		if (!grounded)
			return;

		movement.y = baseSpeed.y;
		unscaledSpeed = movement;
		jumping = true;
    }

    protected override void CalculateSpeed() {
		if (rigidbodyComponent.position.y <= groundLevel && !jumping) {
			movement = Vector2.zero;
			rigidbodyComponent.position = new Vector2(rigidbodyComponent.position.x, groundLevel);
			grounded = true;
			jumping = false;
			return;
		}
		
		if (unscaledSpeed.y >= 0) {
			unscaledSpeed.y += gravityIntensity * getCurrentSlowAmount();
		}
		else {
			unscaledSpeed.y += fallMultiplier * gravityIntensity * getCurrentSlowAmount();
		}

		grounded = false;
		jumping = false;

		movement = unscaledSpeed * Time.fixedDeltaTime;
    }

}