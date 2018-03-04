using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/SwitchSide")]
public class SwitchSideAction : Action {

	public override void Act (StateController controller)
	{
		Move(controller);
	}

	private void Move(StateController controller) {

		SStateController scon = (SStateController)controller;

		scon.thisTransform.position = scon.GetRandomLocation();
		scon.startPosition = scon.thisTransform.position;
	}
}
