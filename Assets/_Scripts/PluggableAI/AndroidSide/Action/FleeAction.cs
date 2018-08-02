using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Flee")]
public class FleeAction : Action {


	public override void Act (BasicStateMachine controller) {
		Flee(controller);
	}

	private void Flee(BasicStateMachine controller) {

		AStateController ncon = (AStateController)controller;

		float speed = Time.fixedDeltaTime;// * ncon.moveDirection;
		if (ncon.useSlowTime.value && !ncon.slowSoldierSide.value)
			speed *= ncon.slowAmount.value;
		
		Vector2 direction = ncon.thisTransform.position-ncon.aPlayer.position;
		ncon.movement = new Vector2(ncon.thisTransform.position.x,ncon.thisTransform.position.y) 
			+ (new Vector2(direction.normalized.x * ncon.values.speed.x,
							direction.normalized.y * ncon.values.speed.y) * speed);

		ncon.movement = ncon.moveBounds.bounds.ClosestPoint(ncon.movement);
		ncon.rigidBody.MovePosition(ncon.movement);

		if (ncon.thisTransform.position.x < ncon.aPlayer.position.x)
			ncon.moveDirection = -1;
		else
			ncon.moveDirection = 1;
	}
}
