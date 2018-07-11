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
			EditorSceneManager.OpenScene("Assets/_Scenes/Dialogue.unity");
			EditorApplication.isPlaying = true;
		}
	}


}
