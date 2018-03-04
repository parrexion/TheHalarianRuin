using UnityEngine;

/// <summary>
/// Base class used by projectiles to define different ways for movement.
/// </summary>
public abstract class MoveScript : MonoBehaviour {

	[Header("Game speed")]
	public BoolVariable paused;
	public BoolVariable canBeSlowed;
	public BoolVariable slowLeftSide;
	public FloatVariable slowAmount;
	public bool leftSideEffect;

    protected Vector2 speed;
    protected Vector2 direction;
    protected Vector2 movement;
    protected Rigidbody2D rigidbodyComponent;


    void Start() {
		if (rigidbodyComponent == null) 
			rigidbodyComponent = GetComponent<Rigidbody2D>();
	}

    void Update() {
        CalculateMovement();
    }

    /// <summary>
    /// Implements how the projectile should move.
    /// </summary>
    protected virtual void CalculateMovement(){}

    protected virtual void CalculateSpeed(){}

    protected float getCurrentSlowAmount() {
        return (canBeSlowed.value && slowLeftSide.value == leftSideEffect) ? slowAmount.value : 1;
    }

    /// <summary>
    /// Updates the movement every frame.
    /// </summary>
    void FixedUpdate() {

        if (rigidbodyComponent == null) 
			rigidbodyComponent = GetComponent<Rigidbody2D>();

        if (paused.value) {
            rigidbodyComponent.velocity = Vector2.zero;
            return;
        }

        CalculateSpeed();

		rigidbodyComponent.velocity = movement * getCurrentSlowAmount();
    }

    /// <summary>
    /// Sets the speed and direction for the projectile.
    /// </summary>
    /// <param name="baseSpeed"></param>
    /// <param name="rotation"></param>
    public abstract void setSpeed(Vector2 baseSpeed, float rotation);

}