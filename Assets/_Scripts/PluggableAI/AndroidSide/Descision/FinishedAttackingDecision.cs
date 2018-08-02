using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/FinishedAttacking")]
public class FinishedAttackingDecision : Decision {

	public override bool Decide(BasicStateMachine controller) {

		StateController con = (StateController)controller;
		if (con.stateTimeElapsed >= con.values.meleeTimeStartup) {
			return true;
		}

		return false;
	}
}
