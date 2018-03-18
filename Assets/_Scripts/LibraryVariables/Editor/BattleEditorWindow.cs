using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class BattleEditorWindow {

	public ScrObjLibraryVariable battleLibrary;
	public BattleEntry battleValues;
	private GUIContent[] currentEntryList;

	// Display screen
	Rect dispRect = new Rect();
	Rect dispRect2 = new Rect();
	Texture2D dispTex;
	Vector2 dispScrollPos;
	private string[] toolbarStrings = new string[] {"Overworld", "Dialogue", "Battle"};

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 selScrollPos;
	string battleUuid;
	int selBattle = -1;
	string filterStr = "";


	public BattleEditorWindow(ScrObjLibraryVariable entries, BattleEntry container){
		battleLibrary = entries;
		battleValues = container;
		LoadLibrary();
	}

	void LoadLibrary() {

		Debug.Log("Loading battle library...");

		battleLibrary.GenerateDictionary();

		Debug.Log("Finished loading battle library");

		InitializeWindow();
	}

	public void InitializeWindow() {
		dispTex = new Texture2D(1, 1);
		dispTex.SetPixel(0, 0, new Color(0.3f, 0.6f, 0.4f));
		dispTex.Apply();

		selectTex = new Texture2D(1, 1);
		selectTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f));
		selectTex.Apply();

		battleValues.ResetValues();
		currentEntryList = battleLibrary.GetRepresentations("","");
		filterStr = "";
	}

	public void DrawWindow() {
		GUILayout.BeginHorizontal();
		GUILayout.Label("Battle Editor", EditorStyles.boldLabel);
		if (selBattle != -1) {
			if (GUILayout.Button("Save Battle")){
				SaveSelectedBattle();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selBattle != -1)
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

		dispRect2.x = 200;
		dispRect2.y = 50;
		dispRect2.width = Screen.width - 220;
		dispRect2.height = Screen.height - 50;
	}

	void DrawBackgrounds() {
		GUI.DrawTexture(selectRect, selectTex);
		GUI.DrawTexture(dispRect, dispTex);
	}

	void DrawEntryList() {
		GUILayout.BeginArea(selectRect);
		GUILayout.Space(5);
		EditorGUIUtility.labelWidth = 80;

		string oldFilter = filterStr;
		filterStr = EditorGUILayout.TextField("Filter", filterStr);
		if (filterStr != oldFilter)
			currentEntryList = battleLibrary.GetRepresentations("",filterStr);

		selScrollPos = EditorGUILayout.BeginScrollView(selScrollPos, GUILayout.Width(selectRect.width), 
							GUILayout.Height(selectRect.height-110));

		int oldSelected = selBattle;
		selBattle = GUILayout.SelectionGrid(selBattle, currentEntryList,1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selBattle) {
			GUI.FocusControl(null);
			SelectBattle();
		}

		battleUuid = EditorGUILayout.TextField("Battle uuid", battleUuid);
		if (GUILayout.Button("Create new")) {
			InstansiateBattle();
		}
		if (GUILayout.Button("Delete battle")) {
			DeleteBattle();
		}
		EditorGUIUtility.labelWidth = 0;

		GUILayout.EndArea();
	}

	void DrawDisplayWindow() {
		GUILayout.BeginArea(dispRect2);
		dispScrollPos = GUILayout.BeginScrollView(dispScrollPos, GUILayout.Width(dispRect2.width), 
							GUILayout.Height(dispRect.height-45));

		EditorGUILayout.SelectableLabel("Selected Battle:   " + battleValues.uuid, EditorStyles.boldLabel);
		battleValues.entryName = EditorGUILayout.TextField("Battle Name", battleValues.entryName);

		GUILayout.Label("After battle", EditorStyles.boldLabel);
		battleValues.nextLocation = (BattleEntry.NextLocation)GUILayout.Toolbar((int)battleValues.nextLocation,toolbarStrings);
		switch (battleValues.nextLocation)
		{
			case BattleEntry.NextLocation.OVERWORLD:
				battleValues.changePosition = EditorGUILayout.Toggle("Change player position", battleValues.changePosition);
				if (battleValues.changePosition) {
					battleValues.playerArea = (Constants.OverworldArea)EditorGUILayout.EnumPopup("Overworld Area",battleValues.playerArea);
					battleValues.playerPosition = EditorGUILayout.Vector2Field("Player Position", battleValues.playerPosition);
				}
				break;
			case BattleEntry.NextLocation.DIALOGUE:
				battleValues.nextDialogue = (DialogueEntry)EditorGUILayout.ObjectField("Next Dialogue",battleValues.nextDialogue, typeof(DialogueEntry),false);
				break;
			case BattleEntry.NextLocation.BATTLE:
				battleValues.nextBattle = (BattleEntry)EditorGUILayout.ObjectField("Next battle", battleValues.nextBattle, typeof(BattleEntry),false);
				break;
		}
		GUILayout.Space(10);
		GUILayout.Label("Battle values", EditorStyles.boldLabel);
		battleValues.escapeButtonEnabled = EditorGUILayout.Toggle("Escapable Battle", battleValues.escapeButtonEnabled);
		GUILayout.Space(10);

		//Tutorial
		battleValues.isTutorial = EditorGUILayout.Toggle("Tutorial Battle", battleValues.isTutorial);
		GUILayout.BeginHorizontal();
		if (battleValues.isTutorial) {
			battleValues.playerInvincible = EditorGUILayout.Toggle("Player invincible", battleValues.playerInvincible);
			battleValues.useSlowTime = EditorGUILayout.Toggle("Use Slow Time", battleValues.useSlowTime);
		}
		GUILayout.EndHorizontal();
		
		if (battleValues.isTutorial) {
			battleValues.backgroundHintLeft = (Sprite)EditorGUILayout.ObjectField("Left side tutorial screen", battleValues.backgroundHintLeft, typeof(Sprite),false);
			battleValues.backgroundHintRight = (Sprite)EditorGUILayout.ObjectField("Right side tutorial screen", battleValues.backgroundHintRight, typeof(Sprite),false);
			battleValues.removeSide = (BattleEntry.RemoveSide)EditorGUILayout.EnumPopup("Battle side to remove for tutotorial screen",battleValues.removeSide);
		}

		GUILayout.Space(20);

		// Background
		GUILayout.Label("Backgrounds", EditorStyles.boldLabel);
		battleValues.backgroundLeft = (Sprite)EditorGUILayout.ObjectField("Left side background", battleValues.backgroundLeft, typeof(Sprite),false);
		battleValues.backgroundRight = (Sprite)EditorGUILayout.ObjectField("Right side background", battleValues.backgroundRight, typeof(Sprite),false);

		// Player stuff
		GUILayout.Label("Equipped module", EditorStyles.boldLabel);
		battleValues.useSpecificModule = EditorGUILayout.Toggle("Use specific module", battleValues.useSpecificModule);

		if (battleValues.useSpecificModule) {
			battleValues.equippedModule[0] = (Module)EditorGUILayout.ObjectField(battleValues.equippedModule[0], typeof(Module),false);
			battleValues.equippedModule[1] = (Module)EditorGUILayout.ObjectField(battleValues.equippedModule[1], typeof(Module),false);
			battleValues.equippedModule[2] = (Module)EditorGUILayout.ObjectField(battleValues.equippedModule[2], typeof(Module),false);
			battleValues.equippedModule[3] = (Module)EditorGUILayout.ObjectField(battleValues.equippedModule[3], typeof(Module),false);
		}

		// Enemies 
		battleValues.randomizeEnemies = EditorGUILayout.Toggle("Randomize enemies", battleValues.randomizeEnemies);
		if (battleValues.randomizeEnemies)
			battleValues.numberOfEnemies = EditorGUILayout.IntField("Number of Enemies", battleValues.numberOfEnemies);
		
		GUILayout.Label("Enemy types", EditorStyles.boldLabel);
		var serializedObject = new SerializedObject(battleValues);
        var property = serializedObject.FindProperty("enemyTypes");
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();

		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
	
	void SelectBattle() {
		// Nothing selected
		if (selBattle == -1) {
			battleValues.ResetValues();
		}
		else {
			// Something selected
			BattleEntry be = (BattleEntry)battleLibrary.GetEntryByIndex(selBattle);
			battleValues.CopyValues(be);
		}
	}

	void SaveSelectedBattle() {
		BattleEntry be = (BattleEntry)battleLibrary.GetEntryByIndex(selBattle);
		be.CopyValues(battleValues);
		Undo.RecordObject(be, "Updated battle");
		EditorUtility.SetDirty(be);
	}

	void InstansiateBattle() {
		GUI.FocusControl(null);
		if (battleLibrary.ContainsID(battleUuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		BattleEntry be = Editor.CreateInstance<BattleEntry>();
		be.name = battleUuid;
		be.uuid = battleUuid;
		be.entryName = battleUuid;
		string path = "Assets/LibraryData/Battles/" + battleUuid + ".asset";

		battleLibrary.InsertEntry(be,0);
		Undo.RecordObject(battleLibrary, "Added battle");
		EditorUtility.SetDirty(battleLibrary);
		AssetDatabase.CreateAsset(be, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		currentEntryList = battleLibrary.GetRepresentations("",filterStr);
		battleUuid = "";
		selBattle = 0;
		SelectBattle();
	}

	void DeleteBattle() {
		GUI.FocusControl(null);
		BattleEntry be = (BattleEntry)battleLibrary.GetEntryByIndex(selBattle);
		string path = "Assets/LibraryData/Battles/" + be.uuid + ".asset";

		battleLibrary.RemoveEntryByIndex(selBattle);
		Undo.RecordObject(battleLibrary, "Deleted battle");
		EditorUtility.SetDirty(battleLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		currentEntryList = battleLibrary.GetRepresentations("",filterStr);

		if (res) {
			Debug.Log("Removed battle: " + be.uuid);
			selBattle = -1;
		}
	}

}
