using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : OWTrigger {

	public DialogueEntry dialogue;
	public StringVariable dialogueUuid;


	public override void Trigger() {
		Debug.Log("Start dialogue: "+ dialogue.name);
		paused.value = true;
		dialogueUuid.value = dialogue.uuid;
		currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
		Deactivate();
		TriggerOtherTriggers();
		startEvent.Invoke();
	}
}
