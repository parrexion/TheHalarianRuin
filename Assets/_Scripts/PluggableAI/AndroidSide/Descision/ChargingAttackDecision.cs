using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/ChargingAttack")]
public class ChargingAttackDecision : Decision {

	public override bool Decide(StateController controller) {

		if (controller.stateTimeElapsed >= controller.values.meleeTimeStartup) {
			return true;
		}

		return false;
	}
}
