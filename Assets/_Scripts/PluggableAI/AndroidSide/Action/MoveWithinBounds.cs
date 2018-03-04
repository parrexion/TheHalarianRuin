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
			Mathf.Clamp(ncon.thisTransform.position.x,Constants.AndroidStartX-Constants.AndroidBorderWidth,Constants.AndroidStartX+Constants.AndroidBorderWidth),
			Mathf.Clamp(ncon.thisTransform.position.y,Constants.AndroidStartY-Constants.AndroidBorderWidth,Constants.AndroidStartY+Constants.AndroidBorderWidth));

		ncon.rigidBody.MovePosition(ncon.movement);
	}
}
