using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKeyButton : MonoBehaviour {
	
	public Canvas nextCanvas;


	// Update is called once per frame
	void Update () {
		if (Input.anyKey) {
			nextCanvas.enabled = true;
			gameObject.SetActive(false);
		}
	}
}
