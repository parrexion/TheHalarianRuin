using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/SwitchSide")]
public class SwitchSideAction : Action {

	public override void Act(BasicStateMachine controller)
	{
		Move(controller);
	}

	private void Move(BasicStateMachine controller) {

		SStateController scon = (SStateController)controller;

		scon.thisTransform.position = scon.GetRandomLocation();
		scon.startPosition = scon.thisTransform.position;
	}
}
