using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/MoveWithinBounds")]
public class MoveWithinBounds : Action {


	public override void Act (BasicStateMachine controller) {
		Move(controller);
	}

	private void Move(BasicStateMachine controller) {
		controller.thisTransform.position = controller.moveBounds.bounds.ClosestPoint(controller.thisTransform.position);
		// controller.rigidBody.MovePosition(controller.movement);
	}
}
