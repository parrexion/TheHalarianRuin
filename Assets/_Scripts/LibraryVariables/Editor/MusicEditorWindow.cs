using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class MusicEditorWindow {

	public ScrObjLibraryVariable musicLibrary;
	public MusicEntry musicValues;
	public ScrObjLibraryVariable sfxLibrary;
	public SfxEntry sfxValues;
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
	public MusicEditorWindow(ScrObjLibraryVariable entries, MusicEntry container, ScrObjLibraryVariable entries2, SfxEntry container2){
		musicLibrary = entries;
		musicValues = container;
		sfxLibrary = entries2;
		sfxValues = container2;
		LoadLibrary();
	}

	void LoadLibrary() {
		Debug.Log("Loading music libraries...");

		musicLibrary.GenerateDictionary();
		sfxLibrary.GenerateDictionary();

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
		sfxValues.ResetValues();
		currentEntryList = musicLibrary.GetRepresentations("","");
		filterStr = "";
	}

	public void DrawWindow() {
		GUILayout.BeginHorizontal();
		GUILayout.Label("Music Editor", EditorStyles.boldLabel);
		if (selMusic != -1) {
			if (GUILayout.Button((soundType == 0) ? "Save Music" : "Save Sfx")){
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
						GUILayout.Height(selectRect.height-150));

		int oldSelected = selMusic;
		selMusic = GUILayout.SelectionGrid(selMusic, currentEntryList,1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selMusic) {
			GUI.FocusControl(null);
			SelectMusic();
		}

		EditorGUIUtility.labelWidth = 110;
		if (soundType == 0) {
			GUILayout.Label("Create new music", EditorStyles.boldLabel);
			uuid = EditorGUILayout.TextField("Music Name", uuid);
			repColor = EditorGUILayout.ColorField("List Color", repColor);
			if (GUILayout.Button("Create music")) {
				InstansiateMusic();
			}
			if (GUILayout.Button("Delete music")) {
				DeleteMusic();
			}
		}
		else if (soundType == 1) {
			GUILayout.Label("Create new sfx", EditorStyles.boldLabel);
			uuid = EditorGUILayout.TextField("Sfx Name", uuid);
			repColor = EditorGUILayout.ColorField("List Color", repColor);
			if (GUILayout.Button("Create sfx")) {
				InstansiateSfx();
			}
			if (GUILayout.Button("Delete sfx")) {
				DeleteSfx();
			}
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindowMusic() {
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

	void DrawDisplayWindowSfx() {
		GUILayout.BeginArea(dispRect);

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Background", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + sfxValues.uuid);
		sfxValues.repColor = EditorGUILayout.ColorField("List color", sfxValues.repColor);

		GUILayout.Space(20);

		sfxValues.entryName = EditorGUILayout.TextField("Name", sfxValues.entryName);
		sfxValues.clip = (AudioClip)EditorGUILayout.ObjectField("Audio Clip", sfxValues.clip, typeof(AudioClip),false);

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
				MusicEntry me = (MusicEntry)musicLibrary.GetEntryByIndex(selMusic);
				musicValues.CopyValues(me);
			}
			else {
				SfxEntry se = (SfxEntry)sfxLibrary.GetEntryByIndex(selMusic);
				sfxValues.CopyValues(se);
			}
		}
	}

	void SaveSelectedMusic() {
		if (soundType == 0) {
			MusicEntry me = (MusicEntry)musicLibrary.GetEntryByIndex(selMusic);
			me.CopyValues(musicValues);
			Undo.RecordObject(me, "Updated music");
			EditorUtility.SetDirty(me);
		}
		else {
			SfxEntry se = (SfxEntry)sfxLibrary.GetEntryByIndex(selMusic);
			se.CopyValues(sfxValues);
			Undo.RecordObject(se, "Updated sfx");
			EditorUtility.SetDirty(se);
		}
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
		SfxEntry se = Editor.CreateInstance<SfxEntry>();
		se.name = uuid;
		se.uuid = uuid;
		se.entryName = uuid;
		se.repColor = repColor;
		string path = "Assets/LibraryData/Sfx/" + uuid + ".asset";

		AssetDatabase.CreateAsset(se, path);
		sfxLibrary.InsertEntry(se,0);
		Undo.RecordObject(sfxLibrary, "Added sfxx");
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
		MusicEntry me = (MusicEntry)musicLibrary.GetEntryByIndex(selMusic);
		string path = "Assets/LibraryData/Music/" + me.uuid + ".asset";

		musicLibrary.RemoveEntryByIndex(selMusic);
		Undo.RecordObject(musicLibrary, "Deleted music");
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
		SfxEntry se = (SfxEntry)sfxLibrary.GetEntryByIndex(selMusic);
		string path = "Assets/LibraryData/Sfx/" + se.uuid + ".asset";

		sfxLibrary.RemoveEntryByIndex(selMusic);
		Undo.RecordObject(sfxLibrary, "Deleted sfx");
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
