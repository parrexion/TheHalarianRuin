using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Flee")]
public class FleeAction : Action {

	public override void Act (StateController controller) {
		Flee(controller);
	}

	private void Flee(StateController controller) {

		AStateController ncon = (AStateController)controller;

		float speed = Time.fixedDeltaTime;// * ncon.moveDirection;
		if (ncon.useSlowTime.value && !ncon.slowSoldierSide.value)
			speed *= ncon.slowAmount.value;
		
		Vector2 direction = ncon.thisTransform.position-ncon.aPlayer.position;
		ncon.movement = new Vector2(ncon.thisTransform.position.x,ncon.thisTransform.position.y) 
			+ (new Vector2(direction.normalized.x * ncon.values.speed.x,
							direction.normalized.y * ncon.values.speed.y) * speed);

		ncon.movement.Set(
			Mathf.Clamp(ncon.movement.x,Constants.AndroidStartX-Constants.AndroidBorderWidth,Constants.AndroidStartX+Constants.AndroidBorderWidth),
			Mathf.Clamp(ncon.movement.y,Constants.AndroidStartY-Constants.AndroidBorderWidth,Constants.AndroidStartY+Constants.AndroidBorderWidth));

		ncon.rigidBody.MovePosition(ncon.movement);

		if (ncon.thisTransform.position.x < ncon.aPlayer.position.x)
			ncon.moveDirection = -1;
		else
			ncon.moveDirection = 1;
	}
}
