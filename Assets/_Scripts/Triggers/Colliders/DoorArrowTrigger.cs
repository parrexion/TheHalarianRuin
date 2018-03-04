using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorArrowTrigger : MonoBehaviour {

	public DoorTrigger interact;
	public bool active = false;
	
	void OnMouseDown() {
        if (!active)
			return;

		interact.EnterDoor();
    }
}
