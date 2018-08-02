using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/Wait")]
public class WaitDecision : Decision {

	public WaitStates waitAction;

	public override bool Decide(BasicStateMachine controller) {
		controller.finishedWaiting = Wait(controller);
		return controller.finishedWaiting;
	}


	private bool Wait(BasicStateMachine controller) {

		if (controller.waitTime == 0) {
			controller.waitTime = Random.Range(controller.waitTimeLimits.minValue,controller.waitTimeLimits.maxValue);
			controller.currentWaitState = controller.GetRandomWaitState();
		}

		if (controller.stateTimeElapsed >= controller.waitTime && controller.currentWaitState == waitAction) {
			return true;
		}

		return false;
	}

}
