using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/MeleeAttack")]
public class MeleeAttackDecision : Decision {

	public override bool Decide(BasicStateMachine controller) {

		AStateController acon = (AStateController)controller;
		float dist = Vector2.Distance(acon.aPlayer.position,acon.thisTransform.position);
		if (dist <= acon.values.meleeRange) {
			acon.hasAttacked = false;
			return true;
		}

		return false;
	}
}
