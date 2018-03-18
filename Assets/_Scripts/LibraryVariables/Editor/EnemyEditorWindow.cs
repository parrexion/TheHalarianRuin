using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class EnemyEditorWindow {

	public ScrObjLibraryVariable enemyLibrary;
	public EnemyEntry enemyValues;
	private GUIContent[] currentEntryList;

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 scrollPos;
	int selEnemy = -1;
	string filterStr = "";

	// Display screen
	Rect dispRect = new Rect();
	Texture2D dispTex;
	Vector2 dispScrollPos;
	Vector2 floatRangeRep;

	//Creation
	string enemyUuid;
	Color repColor = new Color(0,0,0,1f);


	public EnemyEditorWindow(ScrObjLibraryVariable entries, EnemyEntry container){
		enemyLibrary = entries;
		enemyValues = container;
		LoadLibrary();
	}

	void LoadLibrary() {

		Debug.Log("Loading character libraries...");

		enemyLibrary.GenerateDictionary();

		Debug.Log("Finished loading character libraries");

		InitializeWindow();
	}

	public void InitializeWindow() {
		dispTex = new Texture2D(1, 1);
		dispTex.SetPixel(0, 0, new Color(0.1f, 0.4f, 0.6f));
		dispTex.Apply();

		selectTex = new Texture2D(1, 1);
		selectTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f));
		selectTex.Apply();

		enemyValues.ResetValues();
		currentEntryList = enemyLibrary.GetRepresentations("","");
		filterStr = "";
	}


	public void DrawWindow() {

		GUILayout.BeginHorizontal();
		GUILayout.Label("Enemy Editor", EditorStyles.boldLabel);
		if (selEnemy != -1) {
			if (GUILayout.Button("Save Enemy")){
				SaveSelectedEnemy();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selEnemy != -1)
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
		GUILayout.Space(5);
		EditorGUIUtility.labelWidth = 80;

		string oldFilter = filterStr;
		filterStr = EditorGUILayout.TextField("Filter", filterStr);
		if (filterStr != oldFilter)
			currentEntryList = enemyLibrary.GetRepresentations("",filterStr);

		scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(selectRect.width), 
																GUILayout.Height(selectRect.height-150));

		int oldSelected = selEnemy;
		selEnemy = GUILayout.SelectionGrid(selEnemy, currentEntryList,1);
		EditorGUILayout.EndScrollView();
		
		if (oldSelected != selEnemy)
			SelectEnemy();

		EditorGUIUtility.labelWidth = 90;
		GUILayout.Label("Create new enemy", EditorStyles.boldLabel);
		enemyUuid = EditorGUILayout.TextField("Enemy uuid", enemyUuid);
		repColor = EditorGUILayout.ColorField("Display Color", repColor);
		if (GUILayout.Button("Create new")) {
			InstansiateEnemy();
		}
		if (GUILayout.Button("Delete enemy")) {
			DeleteEnemy();
		}
		EditorGUIUtility.labelWidth = 0;

		GUILayout.EndArea();
	}

	void DrawDisplayWindow() {
		GUILayout.BeginArea(dispRect);
		dispScrollPos = GUILayout.BeginScrollView(dispScrollPos, GUILayout.Width(dispRect.width), 
					GUILayout.Height(dispRect.height-25));

		EditorGUILayout.SelectableLabel("Selected Enemy    UUID: " + enemyValues.uuid, EditorStyles.boldLabel);
		enemyValues.repColor = EditorGUILayout.ColorField("List color", enemyValues.repColor);
		enemyValues.entryName = EditorGUILayout.TextField("Name", enemyValues.entryName);

		GUILayout.Label("General", EditorStyles.boldLabel);
		enemyValues.enemyModelN = (Transform)EditorGUILayout.ObjectField("Enemy android model", enemyValues.enemyModelN, typeof(Transform),false);
		enemyValues.enemyModelS = (Transform)EditorGUILayout.ObjectField("Enemy soldier model", enemyValues.enemyModelS, typeof(Transform),false);
		enemyValues.maxhp = EditorGUILayout.IntField("Max HP", enemyValues.maxhp);
		enemyValues.speed = EditorGUILayout.Vector2Field("Movement speed", enemyValues.speed);

		// GUILayout.Label("AI values", EditorStyles.boldLabel);
		var serializedObject = new SerializedObject(enemyValues);
        var property = serializedObject.FindProperty("waitStates");
        serializedObject.Update();
        EditorGUILayout.PropertyField(property, true);
        serializedObject.ApplyModifiedProperties();

		floatRangeRep = new Vector2(enemyValues.waitTimeLimits.minValue, enemyValues.waitTimeLimits.maxValue);
        floatRangeRep = EditorGUILayout.Vector2Field("Wait time limits", floatRangeRep);
        enemyValues.waitTimeLimits.minValue = floatRangeRep.x;
		enemyValues.waitTimeLimits.maxValue = floatRangeRep.y;

		enemyValues.chaseTimeLimit = EditorGUILayout.FloatField("Chase time limit", enemyValues.chaseTimeLimit);
        enemyValues.fleeDistance = EditorGUILayout.FloatField("Flee distance", enemyValues.fleeDistance);
        enemyValues.fleeTimeLimit = EditorGUILayout.FloatField("Flee time limit", enemyValues.fleeTimeLimit);

		GUILayout.Label("Attacking", EditorStyles.boldLabel);
        enemyValues.meleeRange = EditorGUILayout.FloatField("Melee range", enemyValues.meleeRange);
        enemyValues.attackRate = EditorGUILayout.FloatField("Attack rate", enemyValues.attackRate);
        enemyValues.attacks = EditorGUILayout.IntField("Attacks", enemyValues.attacks);
        enemyValues.meleeTimeStartup = EditorGUILayout.FloatField("Melee time startup", enemyValues.meleeTimeStartup);
        enemyValues.meleeTimeAnimation = EditorGUILayout.FloatField("Melee time animation", enemyValues.meleeTimeAnimation);

        GUILayout.Label("Reward", EditorStyles.boldLabel);
        enemyValues.exp = EditorGUILayout.IntField("Exp yield", enemyValues.exp);
        enemyValues.money = EditorGUILayout.IntField("Money yield", enemyValues.money);
        //Add some kind of loot table

		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}
	
	void SelectEnemy() {
		GUI.FocusControl(null);
		// Nothing selected
		if (selEnemy == -1) {
			enemyValues.ResetValues();
		}
		else {
			// Something selected
			EnemyEntry ee = (EnemyEntry)enemyLibrary.GetEntryByIndex(selEnemy);
			enemyValues.CopyValues(ee);
		}
	}

	void SaveSelectedEnemy() {
		EnemyEntry ee = (EnemyEntry)enemyLibrary.GetEntryByIndex(selEnemy);
		ee.CopyValues(enemyValues);
		Undo.RecordObject(ee, "Updated enemy");
		EditorUtility.SetDirty(ee);
	}

	void InstansiateEnemy() {
		GUI.FocusControl(null);
		if (enemyLibrary.ContainsID(enemyUuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		EnemyEntry ee = Editor.CreateInstance<EnemyEntry>();
		ee.name = enemyUuid;
		ee.uuid = enemyUuid;
		ee.entryName = enemyUuid;
		ee.repColor = repColor;
		string path = "Assets/LibraryData/Enemies/" + enemyUuid + ".asset";

		enemyLibrary.InsertEntry(ee,0);
		Undo.RecordObject(enemyLibrary, "Added enemy");
		EditorUtility.SetDirty(enemyLibrary);
		AssetDatabase.CreateAsset(ee, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		currentEntryList = enemyLibrary.GetRepresentations("",filterStr);
		enemyUuid = "";
		selEnemy = 0;
		SelectEnemy();
	}

	void DeleteEnemy() {
		GUI.FocusControl(null);
		EnemyEntry ee = (EnemyEntry)enemyLibrary.GetEntryByIndex(selEnemy);
		string path = "Assets/LibraryData/Enemies/" + ee.uuid + ".asset";

		enemyLibrary.RemoveEntryByIndex(selEnemy);
		Undo.RecordObject(enemyLibrary, "Deleted enemy");
		EditorUtility.SetDirty(enemyLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		currentEntryList = enemyLibrary.GetRepresentations("",filterStr);

		if (res) {
			Debug.Log("Removed enemy: " + ee.uuid);
			selEnemy = -1;
		}
	}
}
