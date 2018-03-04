using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Decisions/HitPlayer")]
public class HitPlayerDecision : Decision {

	public override bool Decide(StateController controller) {

		float dist = Vector2.Distance(controller.aPlayer.position,controller.thisTransform.position);
		if (dist <= controller.thisTransform.localScale.x/2 +0.1f) {
//			Debug.Log("Hit the player");
			return true;
		}

		return false;
	}
}
