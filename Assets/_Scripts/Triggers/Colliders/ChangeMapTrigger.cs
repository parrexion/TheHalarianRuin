using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChangeMapTrigger : OWTrigger {

	public Constants.OverworldArea area;
	public Constants.RoomNumber roomNumber;
	public Vector2 position;
	public IntVariable playerArea;
	public IntVariable playerRoomNumber;
	public FloatVariable posx, posy;


	public override void Trigger() {
		Debug.Log("Moving to area: " + area + ", room: " + roomNumber);
		paused.value = true;
		currentArea.value = (int)area;
		playerArea.value = (int)area;
		playerRoomNumber.value = (int)roomNumber;
		posx.value = position.x;
		posy.value = position.y;
		Debug.Log("Position is now: " + posx.value + ", " + posy.value);
		startEvent.Invoke();
	}
}
