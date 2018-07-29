using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnyKeyButton : MonoBehaviour {
	
	public Canvas nextCanvas;
	public UnityEvent buttonClickedEvent;


	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			nextCanvas.enabled = true;
			buttonClickedEvent.Invoke();
			gameObject.SetActive(false);
		}
	}
}
