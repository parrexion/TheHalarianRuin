using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Melee")]
public class MeleeAction : Action {

	public override void Act (StateController controller) {
		Attack(controller);
	}


	private void Attack(StateController controller) {

		if (!controller.hasAttacked) {
//			Debug.Log("Attack");
			controller.hasAttacked = true;
			controller.attack.Attack(controller);
		}
	}
}
