using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterCloseup : MonoBehaviour {

	public StringVariable characterName;
	public ScrObjEntryReference character;
	public IntVariable poseIndex;

	public Text characterNameBox;
	public Image characterImage;
	public Image poseImage;


	// Use this for initialization
	void Start () {
		UpdateCloseup();
	}
	
	// Update is called once per frame
	public void UpdateCloseup() {

		characterNameBox.text = characterName.value;

		if (character.value == null){
			characterImage.enabled = false;
			poseImage.enabled = false;
		}
		else {
			characterImage.enabled = true;
			poseImage.enabled = true;
			CharacterEntry ce = (CharacterEntry)character.value;
			characterImage.sprite = ce.defaultColor;
			poseImage.sprite = ce.poses[poseIndex.value];
		}
	}
}
