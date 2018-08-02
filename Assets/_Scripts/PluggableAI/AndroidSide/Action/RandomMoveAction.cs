using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/RandomMove")]
public class RandomMoveAction : Action {

	public override void Act(BasicStateMachine controller) {
		Move(controller);
	}

	private void Move(BasicStateMachine controller) {

		Bounds b = controller.moveBounds.bounds;
		if (controller.moveToPoint == new Vector2(-999,-999)) {
			float xpos = Random.Range(b.min.x, b.max.x);
			float ypos = Random.Range(b.min.y, b.max.y);
			controller.moveToPoint = new Vector2(xpos,ypos);
		}

		controller.movement = Vector2.MoveTowards(controller.thisTransform.position,controller.moveToPoint,controller.moveSpeed*Time.fixedDeltaTime);

		controller.movement = b.ClosestPoint(controller.movement);
		controller.rigidBody.MovePosition(controller.movement);

		if (controller.thisTransform.position.x < controller.moveToPoint.x)
			controller.moveDirection = 1;
		else
			controller.moveDirection = -1;
	}
}
