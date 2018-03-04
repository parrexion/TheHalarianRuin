using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour {

	public ScrObjEntryReference dialogueBackground;
	private Image image;

	// Use this for initialization
	void Start () {
		image = GetComponent<Image>();
		UpdateBackground();
	}
	
	// Update is called once per frame
	public void UpdateBackground () {
		if (dialogueBackground.value == null) {
			image.enabled = false;
		}
		else {
			image.sprite = ((BackgroundEntry)(dialogueBackground.value)).sprite;
			image.enabled = true;
		}
	}
}
