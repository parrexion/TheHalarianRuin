using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueScene : MonoBehaviour {

	public StringVariable dialogueUuid;
	public ScrObjEntryReference background;
	public ScrObjEntryReference[] characters;
	public IntVariable[] poses;
	public StringVariable talkingName;
	public ScrObjEntryReference talkingChar;
	public IntVariable talkingPose;
	public StringVariable dialogueText;
	public StringVariable inputText;
	public AudioVariable bkgMusic;
	public AudioVariable sfxClip;
	public FloatVariable effectStartDuration;
	public FloatVariable effectEndDuration;

	private int talkingIndex = -1;
	
	[Header("Animations")]
	public Character[] characterTransforms;

	[Header("Non-dialogue references")]
	public BoolVariable paused;
	public StringVariable battleUuid;
	public IntVariable currentArea;
	public IntVariable playerArea;
	public FloatVariable playerPosX;
	public FloatVariable playerPosY;

	[Header("Events")]
	public UnityEvent mapChangeEvent;
	public UnityEvent backgroundChanged;
	public UnityEvent bkgMusicChanged;
	public UnityEvent playSfx;
	public UnityEvent characterChanged;
	public UnityEvent closeupChanged;
	public UnityEvent dialogueTextChanged;
	public UnityEvent screenFlashEvent;
	public UnityEvent screenShakeEvent;


	public void Reset() {
		Debug.Log("RESET!");
		background.value = null;
		for (int i = 0; i < characters.Length; i++) {
			characters[i].value = null;
		}
		for (int i = 0; i < poses.Length; i++) {
			poses[i].value = -1;
		}
		talkingName.value = "";
		talkingChar.value = null;
		talkingPose.value = -1;
		dialogueText.value = "";
		bkgMusic.value = null;
	}


	public void SetTalkingCharacter(int talkIndex) {
		talkingIndex = talkIndex;
		if (talkingIndex == -1 || talkingIndex == 4) {
			talkingChar.value = null;
			talkingPose.value = -1;
		}
		else {
			talkingChar.value = characters[talkingIndex].value;
			talkingPose.value = poses[talkingIndex].value;
		}
	}
}
