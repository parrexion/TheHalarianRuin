using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicEditorWindow {

	public ScrObjLibraryVariable musicLibrary;
	public MusicEntry musicValues;

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 scrollPos;
	int selMusic = -1;

	// Display screen
	Rect dispRect = new Rect();
	RectOffset dispOffset = new RectOffset();
	Texture2D dispTex;
	Vector2 dispScrollPos;

	//Creation
	string uuid = "";
	Color repColor = new Color(0,0,0,1);


	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="entries"></param>
	/// <param name="container"></param>
	public MusicEditorWindow(ScrObjLibraryVariable entries, MusicEntry container){
		musicLibrary = entries;
		musicValues = container;
		LoadLibrary();
	}

	void LoadLibrary() {
		Debug.Log("Loading music libraries...");

		musicLibrary.GenerateDictionary();

		Debug.Log("Finished loading music libraries");

		InitializeWindow();
	}

	public void InitializeWindow() {
		selectTex = new Texture2D(1, 1);
		selectTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f));
		selectTex.Apply();

		dispTex = new Texture2D(1, 1);
		dispTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.2f));
		dispTex.Apply();

		dispOffset.right = 10;

		musicValues.ResetValues();
	}


	public void DrawWindow() {

		GUILayout.BeginHorizontal();
		GUILayout.Label("Music Editor", EditorStyles.boldLabel);
		if (selMusic != -1) {
			if (GUILayout.Button("Save Music")){
				SaveSelectedMusic();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selMusic != -1)
			DrawDisplayWindow();
	}

	void GenerateAreas() {

		selectRect.x = 0;
		selectRect.y = 50;
		selectRect.width = 200;
		selectRect.height = Screen.height - 50;

		dispRect.x = 200;
		dispRect.y = 50;
		dispRect.width = Screen.width - 200;
		dispRect.height = Screen.height - 50;
	}

	void DrawBackgrounds() {
		GUI.DrawTexture(selectRect, selectTex);
		GUI.DrawTexture(dispRect, dispTex);
	}

	void DrawEntryList() {
		GUILayout.BeginArea(selectRect);

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(selectRect.width), 
						GUILayout.Height(selectRect.height-130));

		int oldSelected = selMusic;
		selMusic = GUILayout.SelectionGrid(selMusic, musicLibrary.GetRepresentations("",""),1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selMusic) {
			GUI.FocusControl(null);
			SelectMusic();
		}

		EditorGUIUtility.labelWidth = 110;
		GUILayout.Label("Create new music", EditorStyles.boldLabel);
		uuid = EditorGUILayout.TextField("Music Name", uuid);
		repColor = EditorGUILayout.ColorField("List Color", repColor);
		if (GUILayout.Button("Create new")) {
			InstansiateMusic();
		}
		if (GUILayout.Button("Delete music")) {
			DeleteMusic();
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindow() {
		GUILayout.BeginArea(dispRect);

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Background", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + musicValues.uuid);
		musicValues.repColor = EditorGUILayout.ColorField("List color", musicValues.repColor);

		GUILayout.Space(20);

		musicValues.entryName = EditorGUILayout.TextField("Name", musicValues.entryName);
		musicValues.clip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", musicValues.clip, typeof(AudioClip),false);

		GUILayout.EndArea();
	}

	void SelectMusic() {
		if (selMusic == -1) {
			// Nothing selected
			musicValues.ResetValues();
		}
		else {
			// Something selected
			MusicEntry me = (MusicEntry)musicLibrary.GetEntryByIndex(selMusic);
			musicValues.CopyValues(me);
		}
	}

	void SaveSelectedMusic() {
		MusicEntry me = (MusicEntry)musicLibrary.GetEntryByIndex(selMusic);
		me.CopyValues(musicValues);
		Undo.RecordObject(me, "Updated music");
		EditorUtility.SetDirty(me);
	}

	void InstansiateMusic() {
		GUI.FocusControl(null);
		if (musicLibrary.ContainsID(uuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		MusicEntry me = Editor.CreateInstance<MusicEntry>();
		me.name = uuid;
		me.uuid = uuid;
		me.entryName = uuid;
		me.repColor = repColor;
		string path = "Assets/LibraryData/Music/" + uuid + ".asset";

		AssetDatabase.CreateAsset(me, path);
		musicLibrary.InsertEntry(me,0);
		Undo.RecordObject(musicLibrary, "Added music");
		EditorUtility.SetDirty(musicLibrary);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		uuid = "";
		selMusic = 0;
		SelectMusic();
	}

	void DeleteMusic() {
		GUI.FocusControl(null);
		MusicEntry me = (MusicEntry)musicLibrary.GetEntryByIndex(selMusic);
		string path = "Assets/LibraryData/Music/" + me.uuid + ".asset";

		musicLibrary.RemoveEntryByIndex(selMusic);
		Undo.RecordObject(musicLibrary, "Deleted music");
		EditorUtility.SetDirty(musicLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		if (res) {
			Debug.Log("Removed music: " + me.uuid);
			selMusic = -1;
		}
	}
}
