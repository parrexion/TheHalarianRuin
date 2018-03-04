using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/WaitChase")]
public class WaitChaseDecision : Decision {

	public override bool Decide(StateController controller) {
		controller.finishedWaiting = Wait(controller);
		return controller.finishedWaiting;
	}


	private bool Wait(StateController controller) {

		if (controller.waitTime == 0) {
			controller.waitTime = Random.Range(controller.values.waitTimeLimits.minValue,controller.values.waitTimeLimits.maxValue);
			controller.currentWaitState = controller.GetRandomWaitState();
		}

		if (controller.stateTimeElapsed >= controller.waitTime && controller.currentWaitState == StateController.WaitStates.CHASE) {
			return true;
		}

		return false;
	}
}
