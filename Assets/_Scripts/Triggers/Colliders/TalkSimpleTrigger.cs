using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkSimpleTrigger : OWTrigger {

	public SpriteRenderer talkSprite;
	public PopupTrigger chat;

	public OneLiner[] dialogueLines;
	private int currentDialogue;

	public StringVariable startText;
	public StringVariable showText;
	public StringVariable talkingName;
	public ScrObjEntryReference talkingCharacter;
	public IntVariable talkingPose;


	public override void Trigger() {
		chat.active = true;
		talkSprite.enabled = true;
		currentDialogue = 0;
	}

	void OnTriggerExit2D(Collider2D otherCollider){
		if (!active)
			return;

		chat.active = false;
		talkSprite.enabled = false;
	}

	public override void IngameTrigger() {
		OneLiner one = dialogueLines[currentDialogue];
		currentDialogue = (currentDialogue +1) % dialogueLines.Length;

		startText.value = "";
		showText.value = one.text;
		talkingName.value = one.character.entryName;
		talkingCharacter.value = one.character;
		talkingPose.value = one.pose;
		startEvent.Invoke();
	}
}
