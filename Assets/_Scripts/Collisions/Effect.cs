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

	[Header("Effect Steps")]
	public EffectStep[] steps;
	protected int currentStep = 0;
	private float currentTime = 0f;
	private float deactivateTime;
	private SpriteRenderer rend;


	void Start () {
		rend = GetComponent<SpriteRenderer>();
		currentStep = 0;

		AdditionalSetup();
		SetupCurrentStep();
	}

	protected virtual void AdditionalSetup() { }

	/// <summary>
	/// Update the current lifetime of the effect.
	/// </summary>
	void Update() {
		if (paused.value)
			return;

		currentTime += (canBeSlowed.value && slowLeftSide.value == leftSideEffect) ? (Time.deltaTime * slowAmount.value) : Time.deltaTime;

		if (currentTime >= deactivateTime) {
			TriggerNextStep();
		}
	}

	protected void TriggerNextStep() {
		currentStep++;
		if (currentStep < steps.Length)
			SetupCurrentStep();
		else {
			Destroy(gameObject);
		}
	}

	void SetupCurrentStep() {
		EffectStep step = steps[currentStep];
		rend.sprite = step.sprite;
		transform.localScale = new Vector3(step.size.x, step.size.y, 1);
		deactivateTime = step.duration;
		currentTime = 0;
		AdditionalCurrentStepSetup();
	}

	protected virtual void AdditionalCurrentStepSetup() { }

	/// <summary>
	/// Sets the movement of the effect by giving speed and rotation to it
	/// </summary>
	/// <param name="baseSpeed"></param>
	/// <param name="rotation"></param>
	public void SetMovement(Vector2 baseSpeed, float rotation){
		move = GetComponent<MoveScript>();
		if (move != null) {
			move.SetSpeed(baseSpeed, rotation);
		}
	}
}
