using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorTrigger : OWTrigger {

	public SpriteRenderer arrowSprite;
	public DoorArrowTrigger arrow;
	public Constants.OverworldArea area;
	public Vector2 position;
	public IntVariable playerArea;
	public FloatVariable posx, posy;


	public override void Trigger() {
		arrow.active = true;
		arrowSprite.enabled = true;
	}

	void OnTriggerExit2D(Collider2D otherCollider){
		if (!active)
			return;

		arrow.active = false;
		arrowSprite.enabled = false;
	}

	public void EnterDoor() {
		Debug.Log("Moving to area: " + area);
		paused.value = true;
		playerArea.value = (int)area;
		posx.value = position.x;
		posy.value = position.y;
		startEvent.Invoke();
	}
}
