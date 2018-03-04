using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/ArrivedMovepoint")]
public class ArrivedPositionDecision : Decision {

	public override bool Decide(StateController controller) {

		AStateController ncon = (AStateController)controller;
		float dist = Vector2.Distance(ncon.moveToPoint,controller.thisTransform.position);
		if (dist <= 0.1f) {
			return true;
		}

		return false;
	}
}
