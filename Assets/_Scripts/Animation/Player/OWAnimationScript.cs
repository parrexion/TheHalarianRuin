using System.Collections;
using UnityEngine;

/// <summary>
/// Implementation of the animation script used by characters in the overworld.
/// </summary>
public class OWAnimationScript : AnimationScript {


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

	}

}
