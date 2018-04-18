using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SonglistEditorWindow {

	public ScrObjLibraryVariable musicLibrary;
	public MusicList musicValues;
	public ScrObjLibraryVariable sfxLibrary;
	public SfxList sfxValues;
	private GUIContent[] currentEntryList;

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 scrollPos;
	int selMusic = -1;
	string filterStr = "";

	// Display screen
	Rect dispRect = new Rect();
	RectOffset dispOffset = new RectOffset();
	Texture2D dispTex;
	Vector2 dispScrollPos;

	//Creation
	string uuid = "";
	Color repColor = new Color(0,0,0,1);
	string[] soundTypeStrings = new string[]{"MUSIC", "SFX"};
	int soundType = 0;


	/// <summary>
	/// Constructor for the editor window.
	/// </summary>
	/// <param name="entries"></param>
	/// <param name="container"></param>
	public SonglistEditorWindow(ScrObjLibraryVariable entries, MusicList container, ScrObjLibraryVariable entries2, SfxList container2){
		musicLibrary = entries;
		musicValues = container;
		sfxLibrary = entries2;
		sfxValues = container2;
		LoadLibrary();
	}

	void LoadLibrary() {
		Debug.Log("Loading song list libraries...");

		musicLibrary.GenerateDictionary();
		sfxLibrary.GenerateDictionary();

		Debug.Log("Finished loading song list libraries");

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
		sfxValues.ResetValues();
		currentEntryList = musicLibrary.GetRepresentations("","");
		filterStr = "";
	}

	public void DrawWindow() {
		GUILayout.BeginHorizontal();
		GUILayout.Label("Songlist Editor", EditorStyles.boldLabel);
		if (selMusic != -1) {
			if (GUILayout.Button((soundType == 0) ? "Save Music list" : "Save Sfx list")){
				SaveSelectedMusic();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selMusic != -1){
			if (soundType == 0)
				DrawDisplayWindowMusic();
			else
				DrawDisplayWindowSfx();
		}
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
		GUILayout.Space(5);

		int oldType = soundType;
		soundType = GUILayout.Toolbar(soundType, soundTypeStrings);

		if (oldType != soundType) {
			currentEntryList = (soundType == 0) ? musicLibrary.GetRepresentations("",filterStr) : sfxLibrary.GetRepresentations("",filterStr);
			SelectMusic();
		}

		EditorGUIUtility.labelWidth = 80;

		string oldFilter = filterStr;
		filterStr = EditorGUILayout.TextField("Filter", filterStr);
		if (filterStr != oldFilter)
			currentEntryList = (soundType == 0) ? musicLibrary.GetRepresentations("",filterStr) : sfxLibrary.GetRepresentations("",filterStr);

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(selectRect.width), 
						GUILayout.Height(selectRect.height-170));

		int oldSelected = selMusic;
		selMusic = GUILayout.SelectionGrid(selMusic, currentEntryList,1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selMusic) {
			GUI.FocusControl(null);
			SelectMusic();
		}

		EditorGUIUtility.labelWidth = 110;
		if (soundType == 0) {
			GUILayout.Label("Create new music list", EditorStyles.boldLabel);
			uuid = EditorGUILayout.TextField("Music List Name", uuid);
			repColor = EditorGUILayout.ColorField("List Color", repColor);
			if (GUILayout.Button("Create music list")) {
				InstansiateMusic();
			}
			if (GUILayout.Button("Delete music list")) {
				DeleteMusic();
			}
		}
		else if (soundType == 1) {
			GUILayout.Label("Create new sfx list", EditorStyles.boldLabel);
			uuid = EditorGUILayout.TextField("Sfx List Name", uuid);
			repColor = EditorGUILayout.ColorField("List Color", repColor);
			if (GUILayout.Button("Create sfx list")) {
				InstansiateSfx();
			}
			if (GUILayout.Button("Delete sfx list")) {
				DeleteSfx();
			}
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindowMusic() {
		GUILayout.BeginArea(dispRect);

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Songlist", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + musicValues.uuid);
		musicValues.repColor = EditorGUILayout.ColorField("List color", musicValues.repColor);

		GUILayout.Space(20);

		// musicValues.entryName = EditorGUILayout.TextField("Name", musicValues.entryName);
		// GUILayout.Label("Songs", EditorStyles.boldLabel);
		// var serializedObject = new SerializedObject(musicValues);
        // var property = serializedObject.FindProperty("musicClips");
        // serializedObject.Update();
        // EditorGUILayout.PropertyField(property, true);
        // serializedObject.ApplyModifiedProperties();

		// Sfx list
		GUILayout.Label("Songs", EditorStyles.boldLabel);
		if (GUILayout.Button("Add Song")) {
			musicValues.musicClips.Add(null);
		}

		for (int i = 0; i < musicValues.musicClips.Count; i++) {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("X", GUILayout.Width(30))){
				musicValues.musicClips.RemoveAt(i);
				i--;
			}
			musicValues.musicClips[i] = (MusicEntry)EditorGUILayout.ObjectField(musicValues.musicClips[i], typeof(MusicEntry), false);
			GUILayout.EndHorizontal();
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindowSfx() {
		GUILayout.BeginArea(dispRect);

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Background", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + sfxValues.uuid);
		sfxValues.repColor = EditorGUILayout.ColorField("List color", sfxValues.repColor);

		GUILayout.Space(20);

		// Sfx list
		GUILayout.Label("Sfx", EditorStyles.boldLabel);
		if (GUILayout.Button("Add Sfx")) {
			sfxValues.sfxClips.Add(null);
		}

		for (int i = 0; i < sfxValues.sfxClips.Count; i++) {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("X", GUILayout.Width(30))){
				sfxValues.sfxClips.RemoveAt(i);
				i--;
			}
			sfxValues.sfxClips[i] = (SfxEntry)EditorGUILayout.ObjectField(sfxValues.sfxClips[i], typeof(SfxEntry), false);
			GUILayout.EndHorizontal();
		}

		GUILayout.EndArea();
	}

	void SelectMusic() {
		if (selMusic == -1) {
			// Nothing selected
			if (soundType == 0)
				musicValues.ResetValues();
			else
				sfxValues.ResetValues();
		}
		else {
			// Something selected
			selMusic = Mathf.Min(currentEntryList.Length -1, selMusic);
			if (soundType == 0) {
				MusicList me = (MusicList)musicLibrary.GetEntryByIndex(selMusic);
				musicValues.CopyValues(me);
			}
			else {
				SfxList se = (SfxList)sfxLibrary.GetEntryByIndex(selMusic);
				sfxValues.CopyValues(se);
			}
		}
	}

	void SaveSelectedMusic() {
		if (soundType == 0) {
			MusicList me = (MusicList)musicLibrary.GetEntryByIndex(selMusic);
			me.CopyValues(musicValues);
			Undo.RecordObject(me, "Updated music list");
			EditorUtility.SetDirty(me);
		}
		else {
			SfxList se = (SfxList)sfxLibrary.GetEntryByIndex(selMusic);
			se.CopyValues(sfxValues);
			Undo.RecordObject(se, "Updated sfx list");
			EditorUtility.SetDirty(se);
		}
	}

	void InstansiateMusic() {
		GUI.FocusControl(null);
		if (musicLibrary.ContainsID(uuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		MusicList me = Editor.CreateInstance<MusicList>();
		me.name = uuid;
		me.uuid = uuid;
		me.entryName = uuid;
		me.repColor = repColor;
		string path = "Assets/LibraryData/Songlists/" + uuid + ".asset";

		AssetDatabase.CreateAsset(me, path);
		musicLibrary.InsertEntry(me,0);
		Undo.RecordObject(musicLibrary, "Added music");
		EditorUtility.SetDirty(musicLibrary);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		currentEntryList = musicLibrary.GetRepresentations("",filterStr);
		uuid = "";
		selMusic = 0;
		SelectMusic();
	}
	
	void InstansiateSfx() {
		GUI.FocusControl(null);
		if (sfxLibrary.ContainsID(uuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		SfxList se = Editor.CreateInstance<SfxList>();
		se.name = uuid;
		se.uuid = uuid;
		se.entryName = uuid;
		se.repColor = repColor;
		string path = "Assets/LibraryData/Songlists/" + uuid + ".asset";

		AssetDatabase.CreateAsset(se, path);
		sfxLibrary.InsertEntry(se,0);
		Undo.RecordObject(sfxLibrary, "Added sfx");
		EditorUtility.SetDirty(sfxLibrary);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		currentEntryList = sfxLibrary.GetRepresentations("",filterStr);
		uuid = "";
		selMusic = 0;
		SelectMusic();
	}

	void DeleteMusic() {
		GUI.FocusControl(null);
		MusicList me = (MusicList)musicLibrary.GetEntryByIndex(selMusic);
		string path = "Assets/LibraryData/Songlists/" + me.uuid + ".asset";

		musicLibrary.RemoveEntryByIndex(selMusic);
		Undo.RecordObject(musicLibrary, "Deleted music list");
		EditorUtility.SetDirty(musicLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		
		currentEntryList = musicLibrary.GetRepresentations("",filterStr);

		if (res) {
			Debug.Log("Removed music: " + me.uuid);
			selMusic = -1;
		}
	}

	void DeleteSfx() {
		GUI.FocusControl(null);
		SfxList se = (SfxList)sfxLibrary.GetEntryByIndex(selMusic);
		string path = "Assets/LibraryData/Songlists/" + se.uuid + ".asset";

		sfxLibrary.RemoveEntryByIndex(selMusic);
		Undo.RecordObject(sfxLibrary, "Deleted sfx list");
		EditorUtility.SetDirty(sfxLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
		
		currentEntryList = sfxLibrary.GetRepresentations("",filterStr);

		if (res) {
			Debug.Log("Removed sfx: " + se.uuid);
			selMusic = -1;
		}
	}
}
