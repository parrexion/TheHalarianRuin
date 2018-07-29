using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour {

	public OWTrigger interact;
	public bool active = false;
	
	void OnMouseDown() {
        if (!active)
			return;

		interact.IngameTrigger();
    }
}
