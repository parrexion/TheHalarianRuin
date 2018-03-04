using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleTrigger : OWTrigger {

	public BattleEntry battle;
	public StringVariable battleUuid;


	public override void Trigger() {
		Debug.Log("Start battle: "+ battle.entryName);
		paused.value = true;
		battleUuid.value = battle.uuid;
		// currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
		Deactivate();
		TriggerOtherTriggers();
		startEvent.Invoke();
	}
}
