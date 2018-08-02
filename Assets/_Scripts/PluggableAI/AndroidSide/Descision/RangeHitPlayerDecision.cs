using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/RangeHitPlayer")]
public class RangeHitPlayerDecision : Decision {

	public override bool Decide(BasicStateMachine controller) {

		AStateController ncon = (AStateController)controller;
		float dist = Vector2.Distance(ncon.aPlayer.position,ncon.thisTransform.position);
		if (dist > 3.0f) {
			controller.hasAttacked = false;
			return true;
		}

		return false;
	}
}
