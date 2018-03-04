using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for all effects spawned during battles.
/// This can be anything from projectiles to particles.
/// </summary>
public class Effect : MonoBehaviour {

	protected MoveScript move;

	[Header("Game speed")]
	public BoolVariable paused;
	public BoolVariable canBeSlowed;
	public BoolVariable slowLeftSide;
	public FloatVariable slowAmount;
	public bool leftSideEffect;

	[HideInInspector] public float lifeTime = 1f;
	protected float currentTime = 0f;


	/// <summary>
	/// Update the current lifetime of the effect.
	/// </summary>
	protected virtual void Update() {
		if (paused.value)
			return;

		currentTime += (canBeSlowed.value && slowLeftSide.value == leftSideEffect) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;

		if (currentTime >= lifeTime){
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Sets the movement of the effect by giving speed and rotation to it
	/// </summary>
	/// <param name="baseSpeed"></param>
	/// <param name="rotation"></param>
	public void SetMovement(Vector2 baseSpeed, float rotation){
		move = GetComponent<MoveScript>();
		if (move != null) {
			move.setSpeed(baseSpeed, rotation);
		}
	}
}
