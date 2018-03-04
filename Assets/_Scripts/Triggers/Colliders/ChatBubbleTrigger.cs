using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBubbleTrigger : MonoBehaviour {

	public TalkTrigger interact;
	public bool active = false;
	
	void OnMouseDown() {
        if (!active)
			return;

		interact.StartTalking();
    }
}
