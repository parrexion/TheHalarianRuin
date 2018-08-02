using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/FinishedDecision")]
public class FinishedFleeingDecision : Decision {

	public override bool Decide(BasicStateMachine controller) {

		AStateController ncon = (AStateController)controller;

		if (controller.stateTimeElapsed >= ncon.values.fleeTimeLimit) {
			return true;
		}

		return false;
	}
}
