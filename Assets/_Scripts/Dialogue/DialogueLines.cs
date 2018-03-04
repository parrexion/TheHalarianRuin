using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[RequireComponent(typeof(DialogueScene))]
public class DialogueLines : MonoBehaviour {

	public ScrObjLibraryVariable dialogueLibrary;
	public StringVariable dialogueUuid;
	private DialogueEntry dialogueEntry;

	public IntVariable currentFrame;
	private DialogueScene scene;
	
	public UnityEvent backgroundChanged;
	public UnityEvent bkgMusicChanged;
	public UnityEvent characterChanged;
	public UnityEvent closeupChanged;
	public UnityEvent dialogueTextChanged;


	void Start() {
		scene = GetComponent<DialogueScene>();
		dialogueLibrary.GenerateDictionary();
		dialogueEntry = (DialogueEntry)dialogueLibrary.GetEntry(dialogueUuid.value);
		currentFrame.value = 0;
		scene.SetFromFrame(dialogueEntry.frames[0]);
		Debug.Log("Set frame 0 of dialogue " + dialogueUuid.value);

		backgroundChanged.Invoke();
		bkgMusicChanged.Invoke();
		characterChanged.Invoke();
		closeupChanged.Invoke();
		dialogueTextChanged.Invoke();
	}

	public void NextFrame(){

		currentFrame.value++;

		if (currentFrame.value >= dialogueEntry.size) {
			Debug.Log("Reached the end");
			DialogueAction da = (DAEndDialogue)ScriptableObject.CreateInstance("DAEndDialogue");
			DialogueJsonItem data = new DialogueJsonItem();
			data.entry = dialogueEntry;
			da.Act(scene,data);
		}
		else {
			CompareScenes(dialogueEntry.frames[currentFrame.value]);
		}
	}

	private void CompareScenes(Frame frame) {
		DialogueAction da;
		DialogueJsonItem data;
		if (!ScrObjLibraryEntry.CompareEntries(scene.background.value,frame.background)) {
			da = (DASetBackground)ScriptableObject.CreateInstance("DASetBackground");
			data = new DialogueJsonItem();
			data.entry = frame.background;
			da.Act(scene,data);
			backgroundChanged.Invoke();
		}
		bool changed = false;
		for (int i = 0; i < 4; i++) {
			if (!ScrObjLibraryEntry.CompareEntries(scene.characters[i].value,frame.characters[i]) || scene.poses[i].value != frame.poses[i]) {
				da = (DAAddCharacter)ScriptableObject.CreateInstance("DAAddCharacter");
				data = new DialogueJsonItem();
				data.position1 = i;
				data.entry = frame.characters[i];
				data.value = frame.poses[i];
				da.Act(scene,data);
				changed = true;
			}
		}
		if (changed) {

			Debug.Log("current: ");	
			characterChanged.Invoke();
		}

		changed = false;
		if (!ScrObjLibraryEntry.CompareEntries(scene.talkingChar.value, frame.talkingChar) || scene.talkingPose.value != frame.talkingPose) {
			da = (DAChangeTalking)ScriptableObject.CreateInstance("DAChangeTalking");
			data = new DialogueJsonItem();
			data.entry = frame.talkingChar;
			data.value = frame.talkingPose;
			da.Act(scene,data);
			changed = true;
		}

		if (scene.talkingName.value != frame.talkingName) {
			da = (DASetName)ScriptableObject.CreateInstance("DASetName");
			data = new DialogueJsonItem();
			data.text = frame.talkingName;
			da.Act(scene,data);
			changed = true;
		}

		if (changed)
			closeupChanged.Invoke();

		if (scene.dialogueText.value != frame.dialogueText) {
			da = (DASetText)ScriptableObject.CreateInstance("DASetText");
			data = new DialogueJsonItem();
			data.text = frame.dialogueText;
			da.Act(scene,data);
			dialogueTextChanged.Invoke();
		}

		if (frame.bkgMusic != null) {
			da = (DASetBkgMusic)ScriptableObject.CreateInstance("DASetBkgMusic");
			data = new DialogueJsonItem();
			data.entry = frame.bkgMusic;
			da.Act(scene,data);
			bkgMusicChanged.Invoke();
		}
	}
}
