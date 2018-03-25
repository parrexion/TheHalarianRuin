using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/RandomMove")]
public class RandomMoveAction : Action {

	public override void Act (StateController controller)
	{
		Move(controller);
	}

	private void Move(StateController controller) {

		AStateController ncon = (AStateController)controller;

		if (ncon.moveToPoint == new Vector2(-5*Constants.ANDROID_BORDER_WIDTH,-5*Constants.ANDROID_BORDER_HEIGHT)) {
			float xpos = Random.Range(Constants.ANDROID_START_X-Constants.ANDROID_BORDER_WIDTH,Constants.ANDROID_START_X+Constants.ANDROID_BORDER_WIDTH);
			float ypos = Random.Range(Constants.ANDROID_START_Y-Constants.ANDROID_BORDER_HEIGHT,Constants.ANDROID_START_Y+Constants.ANDROID_BORDER_HEIGHT);
			ncon.moveToPoint = new Vector2(xpos,ypos);
		}

		ncon.movement = Vector2.MoveTowards(ncon.thisTransform.position,ncon.moveToPoint,ncon.values.speed.x*Time.fixedDeltaTime);

		ncon.movement.Set(
			Mathf.Clamp(ncon.movement.x,Constants.ANDROID_START_X-Constants.ANDROID_BORDER_WIDTH,Constants.ANDROID_START_X+Constants.ANDROID_BORDER_WIDTH),
			Mathf.Clamp(ncon.movement.y,Constants.ANDROID_START_Y-Constants.ANDROID_BORDER_WIDTH,Constants.ANDROID_START_Y+Constants.ANDROID_BORDER_WIDTH));

		ncon.rigidBody.MovePosition(ncon.movement);

		if (ncon.thisTransform.position.x < ncon.moveToPoint.x)
			ncon.moveDirection = 1;
		else
			ncon.moveDirection = -1;
	}
}
