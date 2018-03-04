using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class CharacterEditorWindow {

	public ScrObjLibraryVariable characterLibrary;
	public CharacterEntry charValues;
	public SpriteListVariable poseLibrary;

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 scrollPos;
	int selCharacter = -1;

	// Display screen
	Rect dispRect = new Rect();
	RectOffset dispOffset = new RectOffset();
	Texture2D dispTex;
	Vector2 dispScrollPos;

	//Creation
	string uuid = "";
	Color repColor = new Color(0,0,0,1f);


	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="entries"></param>
	/// <param name="container"></param>
	public CharacterEditorWindow(ScrObjLibraryVariable entries, CharacterEntry container, SpriteListVariable poses){
		characterLibrary = entries;
		charValues = container;
		poseLibrary = poses;
		LoadLibrary();
	}

	void LoadLibrary() {

		Debug.Log("Loading character libraries...");

		characterLibrary.GenerateDictionary();

		Debug.Log("Finished loading character libraries");

		InitializeWindow();
	}

	public void InitializeWindow() {
		selectTex = new Texture2D(1, 1);
		selectTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f));
		selectTex.Apply();

		dispTex = new Texture2D(1, 1);
		dispTex.SetPixel(0, 0, new Color(0.8f, 0.5f, 0.2f));
		dispTex.Apply();

		dispOffset.right = 10;

		charValues.ResetValues();
	}


	public void DrawWindow() {

		GUILayout.BeginHorizontal();
		GUILayout.Label("Character Editor", EditorStyles.boldLabel);
		if (selCharacter != -1) {
			if (GUILayout.Button("Save Character")){
				SaveSelectedCharacter();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selCharacter != -1)
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

		int oldSelected = selCharacter;
		selCharacter = GUILayout.SelectionGrid(selCharacter, characterLibrary.GetRepresentations("",""),1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selCharacter)
			SelectCharacter();

		EditorGUIUtility.labelWidth = 110;
		GUILayout.Label("Create new character", EditorStyles.boldLabel);
		uuid = EditorGUILayout.TextField("Character Name", uuid);
		repColor = EditorGUILayout.ColorField("Display Color", repColor);
		if (GUILayout.Button("Create new")) {
			InstansiateCharacter();
		}
		if (GUILayout.Button("Delete character")) {
			DeleteCharacter();
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindow() {
		GUILayout.BeginArea(dispRect);
		dispScrollPos = GUILayout.BeginScrollView(dispScrollPos, GUILayout.Width(dispRect.width), 
							GUILayout.Height(dispRect.height-25));

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Character", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + charValues.uuid);
		charValues.repColor = EditorGUILayout.ColorField("List color", charValues.repColor);

		GUILayout.Space(20);

		charValues.entryName = EditorGUILayout.TextField("Name", charValues.entryName);
		charValues.defaultColor = (Sprite)EditorGUILayout.ObjectField("Default color", charValues.defaultColor, typeof(Sprite),false);

		for (int i = 0; i < poseLibrary.values.Length; i++) {
			EditorGUILayout.ObjectField(poseLibrary.values[i].name, poseLibrary.values[i], typeof(Sprite),false);
		}

		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	void SelectCharacter() {
		GUI.FocusControl(null);
		if (selCharacter == -1) {
			// Nothing selected
			charValues.ResetValues();
		}
		else {
			// Something selected
			CharacterEntry ce = (CharacterEntry)characterLibrary.GetEntryByIndex(selCharacter);
			ce.poses = poseLibrary.values;
			charValues.CopyValues(ce);
		}
	}

	void SaveSelectedCharacter() {
		CharacterEntry ce = (CharacterEntry)characterLibrary.GetEntryByIndex(selCharacter);
		ce.CopyValues(charValues);
		Undo.RecordObject(ce, "Updated character");
		EditorUtility.SetDirty(ce);
	}

	void InstansiateCharacter() {
		GUI.FocusControl(null);
		if (characterLibrary.ContainsID(uuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		CharacterEntry c = Editor.CreateInstance<CharacterEntry>();
		c.name = uuid;
		c.uuid = uuid;
		c.entryName = uuid;
		c.repColor = repColor;
		string path = "Assets/LibraryData/Characters/" + uuid + ".asset";

		AssetDatabase.CreateAsset(c, path);
		characterLibrary.InsertEntry(c, 0);
		Undo.RecordObject(characterLibrary, "Added character");
		EditorUtility.SetDirty(characterLibrary);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		uuid = "";
		selCharacter = 0;
		SelectCharacter();
	}

	void DeleteCharacter() {
		GUI.FocusControl(null);
		CharacterEntry c = (CharacterEntry)characterLibrary.GetEntryByIndex(selCharacter);
		string path = "Assets/LibraryData/Characters/" + c.uuid + ".asset";

		characterLibrary.RemoveEntryByIndex(selCharacter);
		Undo.RecordObject(characterLibrary, "Deleted character");
		EditorUtility.SetDirty(characterLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		if (res) {
			Debug.Log("Removed character: " + c.uuid);
			selCharacter = -1;
		}
	}
}
