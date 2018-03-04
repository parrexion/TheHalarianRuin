using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BackgroundEditorWindow {

	public ScrObjLibraryVariable backgroundLibrary;
	public BackgroundEntry backgroundValues;

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 scrollPos;
	int selBackground = -1;

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
	public BackgroundEditorWindow(ScrObjLibraryVariable entries, BackgroundEntry container){
		backgroundLibrary = entries;
		backgroundValues = container;
		LoadLibrary();
	}

	void LoadLibrary() {
		Debug.Log("Loading background libraries...");

		backgroundLibrary.GenerateDictionary();

		Debug.Log("Finished loading background libraries");

		InitializeWindow();
	}

	public void InitializeWindow() {
		selectTex = new Texture2D(1, 1);
		selectTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f));
		selectTex.Apply();

		dispTex = new Texture2D(1, 1);
		dispTex.SetPixel(0, 0, new Color(0.8f, 0.5f, 0.8f));
		dispTex.Apply();

		dispOffset.right = 10;

		backgroundValues.ResetValues();
	}


	public void DrawWindow() {

		GUILayout.BeginHorizontal();
		GUILayout.Label("Background Editor", EditorStyles.boldLabel);
		if (selBackground != -1) {
			if (GUILayout.Button("Save Background")){
				SaveSelectedBackground();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selBackground != -1)
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

		int oldSelected = selBackground;
		selBackground = GUILayout.SelectionGrid(selBackground, backgroundLibrary.GetRepresentations("",""),1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selBackground) {
			GUI.FocusControl(null);
			SelectBackground();
		}

		EditorGUIUtility.labelWidth = 110;
		GUILayout.Label("Create new background", EditorStyles.boldLabel);
		uuid = EditorGUILayout.TextField("Background Name", uuid);
		repColor = EditorGUILayout.ColorField("List Color", repColor);
		if (GUILayout.Button("Create new")) {
			InstansiateBackground();
		}
		if (GUILayout.Button("Delete background")) {
			DeleteBackground();
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindow() {
		GUILayout.BeginArea(dispRect);

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Background", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + backgroundValues.uuid);
		backgroundValues.repColor = EditorGUILayout.ColorField("List color", backgroundValues.repColor);

		GUILayout.Space(20);

		backgroundValues.entryName = EditorGUILayout.TextField("Name", backgroundValues.entryName);
		backgroundValues.sprite = (Sprite)EditorGUILayout.ObjectField("Default color", backgroundValues.sprite, typeof(Sprite),false);

		GUILayout.EndArea();
	}

	void SelectBackground() {
		if (selBackground == -1) {
			// Nothing selected
			backgroundValues.ResetValues();
		}
		else {
			// Something selected
			BackgroundEntry bke = (BackgroundEntry)backgroundLibrary.GetEntryByIndex(selBackground);
			backgroundValues.CopyValues(bke);
		}
	}

	void SaveSelectedBackground() {
		BackgroundEntry bke = (BackgroundEntry)backgroundLibrary.GetEntryByIndex(selBackground);
		bke.CopyValues(backgroundValues);
		Undo.RecordObject(bke, "Updated background");
		EditorUtility.SetDirty(bke);
	}

	void InstansiateBackground() {
		GUI.FocusControl(null);
		if (backgroundLibrary.ContainsID(uuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		BackgroundEntry bke = Editor.CreateInstance<BackgroundEntry>();
		bke.name = uuid;
		bke.uuid = uuid;
		bke.entryName = uuid;
		bke.repColor = repColor;
		string path = "Assets/LibraryData/Backgrounds/" + uuid + ".asset";

		AssetDatabase.CreateAsset(bke, path);
		backgroundLibrary.InsertEntry(bke,0);
		Undo.RecordObject(backgroundLibrary, "Added background");
		EditorUtility.SetDirty(backgroundLibrary);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		uuid = "";
		selBackground = 0;
		SelectBackground();
	}

	void DeleteBackground() {
		GUI.FocusControl(null);
		BackgroundEntry bke = (BackgroundEntry)backgroundLibrary.GetEntryByIndex(selBackground);
		string path = "Assets/LibraryData/Backgrounds/" + bke.uuid + ".asset";

		backgroundLibrary.RemoveEntryByIndex(selBackground);
		Undo.RecordObject(backgroundLibrary, "Deleted background");
		EditorUtility.SetDirty(backgroundLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		if (res) {
			Debug.Log("Removed background: " + bke.uuid);
			selBackground = -1;
		}
	}
}
