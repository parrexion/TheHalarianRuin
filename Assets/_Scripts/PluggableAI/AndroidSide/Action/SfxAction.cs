using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "PluggableAI/Actions/Sfx")]
public class SfxAction : Action {

	public enum SfxType { CHARGE, ATTACK }

	public SfxType type;


	public override void Act(BasicStateMachine controller) {
		PlaySfx((StateController)controller);
	}


	private void PlaySfx(StateController controller) {

		if (controller.firstTime) {
			Debug.Log("Play sfx!");
			switch (type)
			{
				case SfxType.ATTACK:
					controller.currentSfx.value.Enqueue(controller.values.attackActivateSfx.clip);
					controller.playSfxEvent.Invoke();
					break;
				case SfxType.CHARGE:
					controller.currentSfx.value.Enqueue(controller.values.attackChargeSfx.clip);
					controller.playSfxEvent.Invoke();
					break;
			}
		}
	}
}
