using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TalkSimpleTrigger : OWTrigger {

	public SpriteRenderer talkSprite;
	public PopupTrigger chat;
	public string dialogueLine;
	public StringVariable dialogueShownText;


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
		dialogueShownText.value = dialogueLine;
		startEvent.Invoke();
	}
}
