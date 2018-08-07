using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OneLinerDatabaseWindow : EditorWindow {

	public struct SelectedPose
	{
		public int index;
		public int pose;
	}
	string[] poseList = new string[] { "Normal", "Sad", "Happy", "Angry", "Dead", "Hmm", "Pleased", "Surprised", "Worried", "Grumpy" };
	
	const int uuidWidth = 120;
	const int characterWidth = 150;
	const int poseWidth = 100;
	const int textWidth = 400;

	public OneLinerDatabase database;

	private Vector2 scrollPos = new Vector2();
	private string filterString = "";
	private string uuid = "";


	[MenuItem("Window/One Liner Database")]
	public static void ShowWindow() {
		GetWindow<OneLinerDatabaseWindow>("One Liner Database");
	}

	void OnEnable() {
		EditorSceneManager.sceneOpened += SceneOpenedCallback;
		InitializeWindow();
	}

	void OnDisable() {
		EditorSceneManager.sceneOpened -= SceneOpenedCallback;
	}

	/// <summary>
	/// Re-initializes the editor when a new scene is loaded.
	/// </summary>
	/// <param name="_scene"></param>
	/// <param name="_mode"></param>
	void SceneOpenedCallback(Scene _scene, OpenSceneMode _mode) {
		Debug.Log("SCENE LOADED");
		InitializeWindow();
	}

	/// <summary>
	/// Initializes the variables needed by the editor.
	/// </summary>
	void InitializeWindow() {

	}


	private void OnGUI() {
		GUILayout.Label("Dialogue Entries", EditorStyles.boldLabel);

		filterString = EditorGUILayout.TextField("Filter String", filterString, GUILayout.Width(textWidth));

		GUILayout.BeginHorizontal();
		GUILayout.Space(10);
		GUILayout.Label("Identifier", EditorStyles.boldLabel, GUILayout.Width(uuidWidth));
		GUILayout.Label("Character", EditorStyles.boldLabel, GUILayout.Width(characterWidth));
		GUILayout.Label("Pose", EditorStyles.boldLabel, GUILayout.Width(poseWidth));
		GUILayout.Label("Dialogue Text", EditorStyles.boldLabel, GUILayout.Width(textWidth));
		GUILayout.Space(10);
		GUILayout.Label("Del", EditorStyles.boldLabel);
		GUILayout.EndHorizontal();

		DrawEntries();
		DrawCreateButtons();
		GUILayout.Space(10);
	}

	private void DrawEntries() {
		scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
		GUIStyle style = new GUIStyle(GUI.skin.button);
		style.normal.textColor = Color.red;

		for (int i = 0; i < database.database.Count; i++) {
			if (!string.IsNullOrEmpty(filterString) && !database.database[i].name.Contains(filterString)) {
				continue;
			}

			GUILayout.BeginHorizontal();
			
			// string tmp = EditorGUILayout.DelayedTextField(database.database[i].name, GUILayout.Width(uuidWidth));
			// if (!string.IsNullOrEmpty(tmp))
			// 	database.database[i].name = tmp;
			GUILayout.Label(database.database[i].name, GUILayout.Width(uuidWidth));

			database.database[i].character = (CharacterEntry)EditorGUILayout.ObjectField("", database.database[i].character, typeof(CharacterEntry),false, GUILayout.Width(characterWidth));
			
			if (GUILayout.Button(poseList[database.database[i].pose], GUILayout.Width(poseWidth))) {
				GenericMenu menu = new GenericMenu();
				SelectedPose selPose = new SelectedPose();
				selPose.index = i;
				for (int p = 0; p < poseList.Length; p++) {
					selPose.pose = p;
					menu.AddItem(new GUIContent(poseList[p]), (p == database.database[i].pose), SetPose, selPose);
				}
				menu.ShowAsContext();
			}

			database.database[i].text = EditorGUILayout.TextField(database.database[i].text, GUILayout.Width(textWidth));

			GUILayout.Space(20);
			if (GUILayout.Button("X", style, GUILayout.Width(30))){
				GUI.FocusControl(null);
				database.database.RemoveAt(i);
				i--;
			}

			GUILayout.EndHorizontal();
		}
		EditorGUILayout.EndScrollView();
	}

	/// <summary>
	/// Sets the pose of the character.
	/// </summary>
	/// <param name="selectedPose"></param>
	void SetPose(object selectedPose) {
		SelectedPose pose = (SelectedPose)selectedPose;
		database.database[pose.index].pose = pose.pose;
	}

	private void DrawCreateButtons() {
		GUILayout.Label("Manage Entries", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();

		uuid = EditorGUILayout.TextField("Identifier: ", uuid, GUILayout.Width(textWidth));

		if (GUILayout.Button("Create dialogue", GUILayout.Width(characterWidth))) {
			InstansiateDialogue(uuid);
		}
		GUILayout.EndHorizontal();
	}

	private void InstansiateDialogue(string inputUuid) {
		GUI.FocusControl(null);
		if (string.IsNullOrEmpty(inputUuid))
			return;
		if (database.ContainsID(inputUuid)) {
			Debug.LogError("uuid already exists!");
			return;
		}

		OneLiner one = Editor.CreateInstance<OneLiner>();
		one.name = inputUuid;
		string path = "Assets/LibraryData/OneLiners/" + inputUuid + ".asset";

		database.database.Insert(0, one);
		Undo.RecordObject(database, "Added one liner");
		EditorUtility.SetDirty(database);
		AssetDatabase.CreateAsset(one, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		uuid = "";
	}

}
