using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkTrigger : OWTrigger {

	public SpriteRenderer talkSprite;
	public ChatBubbleTrigger chat;
	public DialogueEntry dialogue;
	public StringVariable dialogueUuid;


	public override void Trigger() {
		chat.active = true;
		talkSprite.enabled = true;
	}

	void OnTriggerExit2D(Collider2D otherCollider){
		if (!active)
			return;

		chat.active = false;
		talkSprite.enabled = false;
	}

	public void StartTalking() {
		Debug.Log("Start dialogue: "+ dialogue.name);
		dialogueUuid.value = dialogue.uuid;
		startEvent.Invoke();
	}
}
