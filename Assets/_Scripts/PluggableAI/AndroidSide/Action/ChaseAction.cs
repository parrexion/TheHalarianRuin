using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Chase")]
public class ChaseAction : Action {

	public override void Act (StateController controller)
	{
		Chase(controller);
	}

	private void Chase(StateController controller) {

		AStateController ncon = (AStateController)controller;

		float speed = ncon.values.speed.x*Time.fixedDeltaTime;// * ncon.moveDirection;
		if (ncon.useSlowTime.value && !ncon.slowSoldierSide.value) {
			speed *= ncon.slowAmount.value;
		}
		
		ncon.movement = Vector2.MoveTowards(ncon.thisTransform.position,ncon.aPlayer.position,speed);

		ncon.movement.Set(
			Mathf.Clamp(ncon.movement.x,Constants.ANDROID_START_X-Constants.ANDROID_BORDER_WIDTH,Constants.ANDROID_START_X+Constants.ANDROID_BORDER_WIDTH),
			Mathf.Clamp(ncon.movement.y,Constants.ANDROID_START_Y-Constants.ANDROID_BORDER_WIDTH,Constants.ANDROID_START_Y+Constants.ANDROID_BORDER_WIDTH));

		ncon.rigidBody.MovePosition(ncon.movement);


		if (ncon.thisTransform.position.x < ncon.aPlayer.position.x)
			ncon.moveDirection = 1;
		else
			ncon.moveDirection = -1;
	}
}
