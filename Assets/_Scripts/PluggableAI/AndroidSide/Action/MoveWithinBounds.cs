using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/MoveWithinBounds")]
public class MoveWithinBounds : Action {

	public override void Act (StateController controller)
	{
		Move(controller);
	}

	private void Move(StateController controller) {

		AStateController ncon = (AStateController)controller;

		ncon.movement.Set(
			Mathf.Clamp(ncon.thisTransform.position.x,Constants.ANDROID_START_X-Constants.ANDROID_BORDER_WIDTH,Constants.ANDROID_START_X+Constants.ANDROID_BORDER_WIDTH),
			Mathf.Clamp(ncon.thisTransform.position.y,Constants.ANDROID_START_Y-Constants.ANDROID_BORDER_WIDTH,Constants.ANDROID_START_Y+Constants.ANDROID_BORDER_WIDTH));

		ncon.rigidBody.MovePosition(ncon.movement);
	}
}
