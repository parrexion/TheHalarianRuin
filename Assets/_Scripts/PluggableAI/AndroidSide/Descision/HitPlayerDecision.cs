using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/HitPlayer")]
public class HitPlayerDecision : Decision {

	public override bool Decide(BasicStateMachine controller) {

		AStateController acon = (AStateController)controller;
		float dist = Vector2.Distance(acon.aPlayer.position,acon.thisTransform.position);
		if (dist <= acon.thisTransform.localScale.x/2 +0.1f) {
//			Debug.Log("Hit the player");
			return true;
		}

		return false;
	}
}
