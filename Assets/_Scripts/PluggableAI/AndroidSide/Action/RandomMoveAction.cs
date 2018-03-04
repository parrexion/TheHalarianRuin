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

		if (ncon.moveToPoint == new Vector2(-5*Constants.AndroidBorderWidth,-5*Constants.AndroidBorderHeight)) {
			float xpos = Random.Range(Constants.AndroidStartX-Constants.AndroidBorderWidth,Constants.AndroidStartX+Constants.AndroidBorderWidth);
			float ypos = Random.Range(Constants.AndroidStartY-Constants.AndroidBorderHeight,Constants.AndroidStartY+Constants.AndroidBorderHeight);
			ncon.moveToPoint = new Vector2(xpos,ypos);
		}

		ncon.movement = Vector2.MoveTowards(ncon.thisTransform.position,ncon.moveToPoint,ncon.values.speed.x*Time.fixedDeltaTime);

		ncon.movement.Set(
			Mathf.Clamp(ncon.movement.x,Constants.AndroidStartX-Constants.AndroidBorderWidth,Constants.AndroidStartX+Constants.AndroidBorderWidth),
			Mathf.Clamp(ncon.movement.y,Constants.AndroidStartY-Constants.AndroidBorderWidth,Constants.AndroidStartY+Constants.AndroidBorderWidth));

		ncon.rigidBody.MovePosition(ncon.movement);

		if (ncon.thisTransform.position.x < ncon.moveToPoint.x)
			ncon.moveDirection = 1;
		else
			ncon.moveDirection = -1;
	}
}
