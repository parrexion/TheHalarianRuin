using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class DialogueSceneEditorWindow : EditorWindow {

	public StringVariable dialogueUUID;

	DialogueEntry selectedDialogue = null;

	public AreaIntVariable currentArea;
	public AreaIntVariable playerArea;
	public DialogueEntry entry;

	int[] indexList = new int[]{0,2,3,1,4};
	int[] reverseIndexList = new int[]{0,3,1,2,4};


	public void DrawGUI() {

		GUILayout.Label("Dialogue selector", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 100;

		HeaderStuff();
	}

	void HeaderStuff() {
		EditorGUILayout.SelectableLabel("Selected Dialogue UUID: " + dialogueUUID.value, EditorStyles.boldLabel);

		selectedDialogue = (DialogueEntry)EditorGUILayout.ObjectField("Dialogue", selectedDialogue, typeof(DialogueEntry),false);

		GUILayout.Space(10);

		if (selectedDialogue != null) {
			if (GUILayout.Button("Set scene")) {
				dialogueUUID.value = selectedDialogue.uuid;
				Undo.RecordObject(dialogueUUID, "Set selected dialogue");
				EditorUtility.SetDirty(dialogueUUID);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		GUILayout.Space(5);

		if (GUILayout.Button("Start Dialogue Scene")) {
			currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
			playerArea.value = (int)Constants.SCENE_INDEXES.TEST_SCENE;
			Scene currentScene = SceneManager.GetActiveScene();
			if (currentScene.name != "Dialogue")
				EditorSceneManager.OpenScene("Assets/_Scenes/Dialogue.unity");
			EditorApplication.isPlaying = true;
		}

		GUILayout.Space(25);

		if (GUILayout.Button("Convert Dialogue")) {
			GenerateActionsFromFrame(selectedDialogue);
			Debug.Log("Conversion finished!");
		}
	}

	private void GenerateActionsFromFrame(DialogueEntry entry) {
		Debug.Log("Generating actions...");

		Frame previousFrame = new Frame();
		Frame currentFrame = entry.frames[0];
		entry.actions = new List<DialogueActionData>();
		DialogueActionData data = null;
		bool change = false;

		// //Start frame
		// data = new DialogueActionData();
		// data.type = DActionType.START_SCENE;
		// data.entries.Add(currentFrame.background);
		// data.entries.Add(currentFrame.bkgMusic);
		// for (int i = 0; i < 4; i++) {
		// 	int index = indexList[i];
		// 	data.entries.Add(currentFrame.characters[i]);
		// 	data.values.Add(currentFrame.poses[i]);
		// }
		// entry.actions.Add(data);

		//Start actions
		data = new DialogueActionData();
		data.type = DActionType.SET_MUSIC;
		data.entries.Add(currentFrame.bkgMusic);
		entry.actions.Add(data);
		data = new DialogueActionData();
		data.type = DActionType.SET_CHARS;
		for (int i = 0; i < 4; i++) {
			int index = indexList[i];
			data.entries.Add(currentFrame.characters[index]);
			data.values.Add(currentFrame.poses[index]);
		}
		entry.actions.Add(data);
		data = new DialogueActionData();
		data.type = DActionType.SET_BKG;
		data.entries.Add(currentFrame.background);
		entry.actions.Add(data);

		previousFrame.background = currentFrame.background;
		previousFrame.bkgMusic = currentFrame.bkgMusic;
		previousFrame.characters = currentFrame.characters;
		previousFrame.poses = currentFrame.poses;

		//The other frames
		int pos = 0;
		while (pos < entry.frames.Count) {
			currentFrame = entry.frames[pos];

			//Background
			if (!ScrObjLibraryEntry.CompareEqual(previousFrame.background,currentFrame.background)) {
				data = new DialogueActionData();
				data.type = DActionType.SET_BKG;
				data.entries.Add(currentFrame.background);
				entry.actions.Add(data);
			}

			//Characters and poses
			change = false;
			data = new DialogueActionData();
			data.type = DActionType.SET_CHARS;
			for (int i = 0; i < 4; i++) {
				if (!ScrObjLibraryEntry.CompareEqual(previousFrame.characters[i],currentFrame.characters[i]) || previousFrame.poses[i] != currentFrame.poses[i]) {
					change = true;
				}
				int index = indexList[i];
				data.entries.Add(currentFrame.characters[index]);
				data.values.Add(currentFrame.poses[index]);
			}
			if (change) {
				entry.actions.Add(data);
			}

			//Background music
			if (currentFrame.bkgMusic != null) {
				data = new DialogueActionData();
				data.type = DActionType.SET_MUSIC;
				data.entries.Add(currentFrame.bkgMusic);
				entry.actions.Add(data);
			}

			//Talking and text
			change = false;
			data = new DialogueActionData();
			if (!ScrObjLibraryEntry.CompareEqual(previousFrame.talkingChar, currentFrame.talkingChar) || previousFrame.talkingPose != currentFrame.talkingPose) {
				change = true;
			}
			data.values.Add((currentFrame.talkingIndex != -1) ? reverseIndexList[currentFrame.talkingIndex] : -1);

			if (previousFrame.talkingName != currentFrame.talkingName) {
				change = true;
			}
			data.text.Add(currentFrame.talkingName);

			if (previousFrame.dialogueText != currentFrame.dialogueText) {
				change = true;
			}
			data.type = DActionType.SET_TEXT;
			data.text.Add(currentFrame.dialogueText);
			data.autoContinue = false;
			data.boolValue = true;

			if (change) {
				entry.actions.Add(data);
			}

			previousFrame = entry.frames[pos];
			pos++;
		}

		//End Dialogue
		data = new DialogueActionData();
		data.type = DActionType.END_SCENE;
		data.autoContinue = false;
		data.entries.Add(entry.dialogueEntry);
		data.entries.Add(entry.battleEntry);
		data.values.Add((int)entry.nextLocation);
		data.values.Add((entry.changePosition) ? 1 : 0);
		data.values.Add((int)entry.nextArea);
		data.values.Add((int)entry.playerPosition.x);
		data.values.Add((int)entry.playerPosition.y);
		entry.actions.Add(data);

		InstansiateDialogue();
	}
	
	private void InstansiateDialogue() {
		Debug.Log("Saving dialogue as a copy...");

		string saveStr = selectedDialogue.uuid;
		DialogueEntry de = Editor.CreateInstance<DialogueEntry>();
		de.CopyValues(selectedDialogue);
		string path = "Assets/LibraryData/Dialogues/Converted/" + saveStr + ".asset";

		AssetDatabase.CreateAsset(de, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		selectedDialogue = null;
	}
}
