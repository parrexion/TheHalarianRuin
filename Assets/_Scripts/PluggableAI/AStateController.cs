using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStateController : StateController {


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
		moveToPoint = new Vector2(-999,-999);
		gameObject.layer = LayerMask.NameToLayer(((State.StateLayer)currentState.stateLayer).ToString());
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

	/// <summary>
	/// Generates a random location around the center of the battle.
	/// </summary>
	/// <returns></returns>
	public override Vector3 GetRandomLocation() {
		float dist = Random.Range(Constants.ENEMY_OFFSET_MIN_ANDROID, Constants.ENEMY_OFFSET_MAX_ANDROID);
		Vector2 offset = Random.insideUnitCircle.normalized * dist;
		return new Vector3(moveBounds.bounds.center.x + offset.x, moveBounds.bounds.center.y + offset.y,0);
	}
}
