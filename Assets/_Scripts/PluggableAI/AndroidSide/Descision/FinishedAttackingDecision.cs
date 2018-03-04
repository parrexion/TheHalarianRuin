using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/FinishedAttacking")]
public class FinishedAttackingDecision : Decision {

	public override bool Decide(StateController controller) {

		if (controller.stateTimeElapsed >= controller.values.meleeTimeAnimation) {
			return true;
		}

		return false;
	}
}
