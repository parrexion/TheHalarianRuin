using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStateController : StateController {

	//Moving
	private int lastTime = 0;
	[HideInInspector] public int moveDirection;

	//Chasing
	[HideInInspector] public Vector2 movement;
	//

	//Move to point
	[HideInInspector] public Vector2 moveToPoint = new Vector2(-5*Constants.AndroidBorderWidth,-5*Constants.AndroidBorderHeight);
	//


	/// /////////////////////////////////////////////////////

	override protected void Start() {
		base.Start();
		if (thisTransform.position.x < aPlayer.position.x)
			moveDirection = 1;
		else
			moveDirection = -1;
	}

	override protected void OnExitState() {
		if (thisTransform.position.x < aPlayer.position.x)
			moveDirection = 1;
		else
			moveDirection = -1;
		moveToPoint = new Vector2(-5*Constants.AndroidBorderWidth,-5*Constants.AndroidBorderHeight);
	}

	override protected void UpdateAnimation() {

		animInfo.attacking = false;
		animInfo.chasing = false;
		animInfo.blocking = false;
		animInfo.hurt = false;
		animInfo.jumping = false;
		animInfo.mouseDirection = 0;

		switch (currentState.stateString) 
		{
		case AnimationScript.StateString.Idle:
			if (lastTime != moveDirection){
				animInfo.mouseDirection = moveDirection;
				lastTime = moveDirection;
			}
			else
				animInfo.mouseDirection = 0;
			break;
		case AnimationScript.StateString.WalkLeft:
			lastTime = moveDirection;
			animInfo.mouseDirection = moveDirection;
			break;
		case AnimationScript.StateString.Chase:
			animInfo.chasing = true;
			lastTime = moveDirection;
			animInfo.mouseDirection = moveDirection;
			break;
		case AnimationScript.StateString.Attack:
			animInfo.attacking = true;
			lastTime = moveDirection;
			animInfo.mouseDirection = moveDirection;
			break;
		default:
			Debug.Log(currentState.stateString.ToString());
			break;
		}

		float speed = (useSlowTime.value && !slowSoldierSide.value) ? slowAmount.value : 1f;
		animScript.UpdateState(animInfo, speed);
	}

	public override Vector3 GetRandomLocation() {
		Vector2 dist = Random.rotation * new Vector2(3,0);
		return new Vector3(Constants.AndroidStartX + dist.x, Constants.AndroidStartY + dist.y,0);
	}
}
