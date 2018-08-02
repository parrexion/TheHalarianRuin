using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkSimpleTrigger : OWTrigger {

	public SpriteRenderer talkSprite;
	public PopupTrigger chat;

	public string dialogueLine;
	public CharacterEntry character;

	public StringVariable startText;
	public StringVariable showText;
	public StringVariable talkingName;
	public ScrObjEntryReference talkingCharacter;


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

	public override void IngameTrigger() {
		startText.value = "";
		showText.value = dialogueLine;
		talkingName.value = character.entryName;
		talkingCharacter.value = character;
		startEvent.Invoke();
	}
}
