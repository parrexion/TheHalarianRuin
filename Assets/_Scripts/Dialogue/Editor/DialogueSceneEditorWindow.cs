using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class DialogueSceneEditorWindow : EditorWindow {

	public StringVariable dialogueUUID;

	DialogueEntry selectedDialogue = null;

	DialogueParser parser;
	public AreaIntVariable currentArea;
	public AreaIntVariable playerArea;
	public ScrObjLibraryVariable backgroundLibrary;
	public ScrObjLibraryVariable characterLibrary;
	public ScrObjLibraryVariable dialogueLibrary;
	public DialogueEntry entry;

	[MenuItem("Window/DialogueSelector")]
	public static void ShowWindow() {
		GetWindow<DialogueSceneEditorWindow>("Dialogue Selector");
	}

	void OnEnable() {
		EditorSceneManager.sceneOpened += SceneOpenedCallback;
		InitializeWindow();
	}

	void OnDisable() {
		EditorSceneManager.sceneOpened -= SceneOpenedCallback;
	}

	void OnGUI() {

		GUILayout.Label("Dialogue selector", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 100;

		HeaderStuff();
	}

	void SceneOpenedCallback( Scene _scene, OpenSceneMode _mode) {
		Debug.Log("SCENE LOADED");
		InitializeWindow();
	}

	void InitializeWindow() {
		parser = new DialogueParser();
		parser.backgroudLibrary = backgroundLibrary;
		parser.characterLibrary = characterLibrary;
		parser.dialogueLibrary = dialogueLibrary;
		parser.entry = entry;
	}

	void HeaderStuff() {
		EditorGUILayout.SelectableLabel("Selected Dialogue    UUID: " + dialogueUUID.value, EditorStyles.boldLabel);

		if (GUILayout.Button("Open Dialogue Scene")) {
			currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
			playerArea.value = (int)Constants.SCENE_INDEXES.ANDROID_BAY;
			EditorSceneManager.OpenScene("Assets/_Scenes/Dialogue.unity");
		}

		GUILayout.Space(5);

		selectedDialogue = (DialogueEntry)EditorGUILayout.ObjectField("Dialogue", selectedDialogue, typeof(DialogueEntry),false);

		if (selectedDialogue != null) {
			if (GUILayout.Button("Set scene")) {
				dialogueUUID.value = selectedDialogue.uuid;
				Undo.RecordObject(dialogueUUID, "Set selected dialogue");
				EditorUtility.SetDirty(dialogueUUID);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		// if (GUILayout.Button("Update dialogueEntry")) {
		// 	parser.UpdateDialogueEntry(dialogueUUID.value);
		// }
	}


}
