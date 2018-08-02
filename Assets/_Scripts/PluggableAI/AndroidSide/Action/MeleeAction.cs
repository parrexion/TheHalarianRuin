using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Melee")]
public class MeleeAction : Action {

	public override void Act (BasicStateMachine controller) {
		Attack((StateController)controller);
	}


	private void Attack(StateController controller) {

		if (!controller.hasAttacked) {
			controller.hasAttacked = true;
			controller.attack.Attack(controller);
		}
	}
}
