using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour {

	public ScrObjEntryReference character;
	public IntVariable poseIndex;

	[SerializeField] private SpriteRenderer characterSprite = null;
	[SerializeField] private SpriteRenderer poseSprite = null;
	

	// Use this for initialization
	void Start () {
		UpdateCharacter();
	}

	public void UpdateCharacter() {
		if (character.value == null){
			characterSprite.enabled = false;
			poseSprite.enabled = false;
		}
		else {
			characterSprite.enabled = true;
			poseSprite.enabled = true;
			CharacterEntry ce = (CharacterEntry)character.value;
			characterSprite.sprite = ce.defaultColor;
			poseSprite.sprite = ce.poses[poseIndex.value];
		}

	}

}
