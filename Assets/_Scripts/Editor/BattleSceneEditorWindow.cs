using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class BattleSceneEditorWindow : EditorWindow {

	public StringVariable battleUUID;

	BattleEntry selectedBattle = null;

	public AreaIntVariable currentArea;
	public AreaIntVariable playerArea;


	public void DrawGUI() {

		GUILayout.Label("Battle selector", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 100;

		HeaderStuff();
	}

	void HeaderStuff() {
		EditorGUILayout.SelectableLabel("Selected Battle UUID: " + battleUUID.value, EditorStyles.boldLabel);

		selectedBattle = (BattleEntry)EditorGUILayout.ObjectField("Battle", selectedBattle, typeof(BattleEntry),false);

		GUILayout.Space(10);

		if (selectedBattle != null) {
			if (GUILayout.Button("Set battle")) {
				battleUUID.value = selectedBattle.uuid;
				Undo.RecordObject(battleUUID, "Set selected battle");
				EditorUtility.SetDirty(battleUUID);
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
			}
		}

		GUILayout.Space(5);

		if (GUILayout.Button("Start Battle Scene")) {
			currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
			playerArea.value = (int)Constants.SCENE_INDEXES.TEST_SCENE;
			Scene currentScene = SceneManager.GetActiveScene();
			if (currentScene.name != "Battle")
				EditorSceneManager.OpenScene("Assets/_Scenes/Battle.unity");
			EditorApplication.isPlaying = true;
		}

	}

}
