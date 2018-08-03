using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Implementation of the animation script used by characters in the overworld.
/// </summary>
public class OWAnimationScript : AnimationScript {

	public float footstepDelay = 0.3f;
	public SfxList footstepSfxs;
	public AudioQueueVariable currentSfx;
	public UnityEvent playSfx;
	private float currentTime;


	public override void UpdateState(AnimationInformation info, float speed) {
		animator.speed = speed;

		if (info.walkDirection == Constants.Direction.LEFT) {
			animator.Play(kWalkLAnim);
		}
		else if (info.walkDirection == Constants.Direction.RIGHT) {
			animator.Play(kWalkRAnim);
		}
		else if (info.walkDirection == Constants.Direction.UP) {
			animator.Play(kWalkUAnim);
		}
		else if (info.walkDirection == Constants.Direction.DOWN) {
			animator.Play(kWalkDAnim);
		}
		else {
			// animator.Play(kIdleAnim);
			animator.speed = 0;
		}

		if (footstepSfxs == null)
			return;

		currentTime += Time.deltaTime * animator.speed;
		if (currentTime >= footstepDelay) {
			currentTime -= footstepDelay;
			currentSfx.value.Enqueue(footstepSfxs.RandomClip());
			Debug.Log("STEP   " + currentSfx.value.Count);
			playSfx.Invoke();
		}
	}

}
