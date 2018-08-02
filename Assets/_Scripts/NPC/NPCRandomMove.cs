using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCRandomMove : BasicStateMachine {

	public IntVariable npcMoveSpeed;

	public WaitStates[] availableWaits;

	[Header("Animation")]
	public int direction;


	private void Start() {
		animInfo = new AnimationInformation();
		waitTimeLimits = new RangedFloat(3f,5f);
		moveSpeed = npcMoveSpeed.value;
	}

    public override WaitStates GetRandomWaitState(){
		int select = Random.Range(0,availableWaits.Length);
		return availableWaits[select];
	}

    protected override void OnExitState() {
		moveToPoint = new Vector2(-999,-999);
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

		animScript.UpdateState(animInfo, 1f);
	}


}
