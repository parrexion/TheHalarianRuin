using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueScene : MonoBehaviour {

	public StringVariable dialogueUuid;
	public ScrObjEntryReference background;
	[SerializeField] private ScrObjEntryReference character0;
	[SerializeField] private ScrObjEntryReference character1;
	[SerializeField] private ScrObjEntryReference character2;
	[SerializeField] private ScrObjEntryReference character3;
	[SerializeField] private IntVariable pose0;
	[SerializeField] private IntVariable pose1;
	[SerializeField] private IntVariable pose2;
	[SerializeField] private IntVariable pose3;
	public StringVariable talkingName;
	public ScrObjEntryReference talkingChar;
	public IntVariable talkingPose;
	public StringVariable dialogueText;
	public AudioVariable bkgMusic;

	[HideInInspector] public ScrObjEntryReference[] characters;
	[HideInInspector] public IntVariable[] poses;

	[Header("Non-dialogue references")]
	public BoolVariable paused;
	public StringVariable battleUuid;
	public IntVariable currentArea;
	public IntVariable playerArea;
	public FloatVariable playerPosX;
	public FloatVariable playerPosY;
	public UnityEvent mapChangeEvent;


	void Start() {
		characters = new ScrObjEntryReference[]{ character0, character1, character2, character3 };
		poses = new IntVariable[]{pose0, pose1, pose2, pose3};

		background.value = null;
		character0.value = null;
		character1.value = null;
		character2.value = null;
		character3.value = null;
		pose0.value = -1;
		pose1.value = -1;
		pose2.value = -1;
		pose3.value = -1;
		talkingName.value = "";
		talkingChar.value = null;
		talkingPose.value = -1;
		dialogueText.value = "";
	}

	public void SetFromFrame(Frame f) {
		background.value = f.background;
		characters[0].value = f.characters[0];
		characters[1].value = f.characters[1];
		characters[2].value = f.characters[2];
		characters[3].value = f.characters[3];
		pose0.value = f.poses[0];
		pose1.value = f.poses[1];
		pose2.value = f.poses[2];
		pose3.value = f.poses[3];
		talkingName.value = f.talkingName;
		talkingChar.value = f.talkingChar;
		talkingPose.value = f.talkingPose;
		dialogueText.value = f.dialogueText;
		bkgMusic.value = (f.bkgMusic) ? f.bkgMusic.clip : null;
	}

}
