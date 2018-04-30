using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SStateController : StateController {

	[HideInInspector] public bool leftSide = false;
	private int lastTime = 0;

	/// /////////////////////////////////////////////////////

	override protected void OnExitState() {

	}

	override protected void UpdateAnimation() {
		animInfo.attacking = false;
		animInfo.chasing = false;
		animInfo.blocking = false;
		animInfo.hurt = false;
		animInfo.jumping = false;
		if (leftSide)
			animInfo.mouseDirection = 1;
		else
			animInfo.mouseDirection = -1;

		switch (currentState.stateString) 
		{
		case AnimationScript.StateString.Idle:
			if (lastTime != animInfo.mouseDirection){
				lastTime = animInfo.mouseDirection;
			}
			else
				animInfo.mouseDirection = 0;
			break;
		case AnimationScript.StateString.WalkLeft:
			break;
		case AnimationScript.StateString.Attack:
			animInfo.attacking = true;
			break;
		default:
			Debug.Log(currentState.stateString.ToString());
			break;
		}

		float speed = (useSlowTime.value && slowSoldierSide.value) ? slowAmount.value : 1f;
		animScript.UpdateState(animInfo, speed);
	}

	/// <summary>
	/// Generates a random location on one of the sides around the soldier.
	/// </summary>
	/// <returns></returns>
	public override Vector3 GetRandomLocation(){
		leftSide = !leftSide;
		float xpos = 0f;
		float ypos = 0f;
		if (leftSide)
			xpos = Random.Range(Constants.SOLDIER_START_X-Constants.ENEMY_OFFSET_XMAX_SOLDIER,Constants.SOLDIER_START_X-Constants.ENEMY_OFFSET_XMIN_SOLDIER);
		else
			xpos = Random.Range(Constants.SOLDIER_START_X+Constants.ENEMY_OFFSET_XMIN_SOLDIER,Constants.SOLDIER_START_X+Constants.ENEMY_OFFSET_XMAX_SOLDIER);
		ypos = Random.Range(Constants.SOLDIER_START_Y+Constants.ENEMY_OFFSET_YMIN_SOLDIER,Constants.SOLDIER_START_Y+Constants.ENEMY_OFFSET_YMAX_SOLDIER);

		return new Vector3(xpos,ypos,0);
	}
}
