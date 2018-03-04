using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCloseup : MonoBehaviour {

	public StringVariable characterName;
	public ScrObjEntryReference character;
	public IntVariable poseIndex;

	public Text characterNameBox;
	public SpriteRenderer characterRenderer;
	public SpriteRenderer poseRenderer;


	// Use this for initialization
	void Start () {
		UpdateCloseup();
	}
	
	// Update is called once per frame
	public void UpdateCloseup() {

		characterNameBox.text = characterName.value;

		if (character.value == null){
			characterRenderer.enabled = false;
			poseRenderer.enabled = false;
		}
		else {
			characterRenderer.enabled = true;
			poseRenderer.enabled = true;
			CharacterEntry ce = (CharacterEntry)character.value;
			characterRenderer.sprite = ce.defaultColor;
			poseRenderer.sprite = ce.poses[poseIndex.value];
		}
	}
}
