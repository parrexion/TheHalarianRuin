using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ModuleEditorWindow {

	public ScrObjLibraryVariable moduleLibrary;
	public Module moduleBase;
	private ModuleValues kv;

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 scrollPos;
	int selModule = -1;

	// Display screen
	Rect dispRect = new Rect();
	RectOffset dispOffset = new RectOffset();
	Texture2D dispTex;
	Vector2 dispScrollPos;
	int valuePage = 0;
	string[] pageStrings = {"VALUES", "ACTIVATION", "EFFECT"};
	ModuleActivation activation;
	ModuleEffect effect;

	//Creation
	string uuid = "";
	Color repColor = new Color(0,0,0,1f);


	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="entries"></param>
	/// <param name="container"></param>
	public ModuleEditorWindow(ScrObjLibraryVariable entries, Module container){
		moduleLibrary = entries;
		moduleBase = container;
		LoadLibrary();
	}

	void LoadLibrary() {

		Debug.Log("Loading module libraries...");

		moduleLibrary.GenerateDictionary();

		Debug.Log("Finished loading module libraries");

		InitializeWindow();
	}

	public void InitializeWindow() {
		selectTex = new Texture2D(1, 1);
		selectTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f));
		selectTex.Apply();

		dispTex = new Texture2D(1, 1);
		dispTex.SetPixel(0, 0, new Color(0.1f, 0.6f, 0.6f));
		dispTex.Apply();

		dispOffset.right = 10;

		moduleBase.ResetValues();
	}


	public void DrawWindow() {

		GUILayout.BeginHorizontal();
		GUILayout.Label("Module Editor", EditorStyles.boldLabel);
		if (selModule != -1) {
			if (GUILayout.Button("Save Module")){
				SaveSelectedModule();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selModule != -1)
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
						GUILayout.Height(selectRect.height-110));

		int oldSelected = selModule;
		selModule = GUILayout.SelectionGrid(selModule, moduleLibrary.GetRepresentations("",""),1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selModule)
			SelectModule();

		EditorGUIUtility.labelWidth = 110;
		GUILayout.Label("Create new module", EditorStyles.boldLabel);
		uuid = EditorGUILayout.TextField("Module Name", uuid);

		if (GUILayout.Button("Create new")) {
			InstansiateModule();
		}
		if (GUILayout.Button("Delete module")) {
			DeleteModule();
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindow() {
		kv = moduleBase.values;
		GUILayout.BeginArea(dispRect);
		dispScrollPos = GUILayout.BeginScrollView(dispScrollPos, GUILayout.Width(dispRect.width), 
							GUILayout.Height(dispRect.height-25));

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Module", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + moduleBase.uuid);
		moduleBase.entryName = EditorGUILayout.TextField("Name", moduleBase.entryName);
		moduleBase.icon = (Sprite)EditorGUILayout.ObjectField("Module Icon", moduleBase.icon, typeof(Sprite),false);

		GUILayout.Label("Module values", EditorStyles.boldLabel);
		valuePage = GUILayout.Toolbar(valuePage, pageStrings);

EditorGUIUtility.labelWidth = 150;

		switch(valuePage)
		{
			case 0: DrawUsagePart(); break;
			case 1: DrawActivationPart(); break;
			case 2: DrawEffectPart(); break;
		}

EditorGUIUtility.labelWidth = 100;

		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	void DrawUsagePart() {
		//Usage values
		GUILayout.Label("Usage Values", EditorStyles.boldLabel);
		kv.startCooldownTime = EditorGUILayout.FloatField("Start Cooldown Delay", kv.startCooldownTime);
		kv.maxCharges = EditorGUILayout.IntField("Max Charges", kv.maxCharges);
		kv.delay = EditorGUILayout.FloatField("Usage Delay", kv.delay);
		kv.cooldown = EditorGUILayout.FloatField("Recharge Cooldown", kv.cooldown);

		GUILayout.Space(10);

		//Other values
		GUILayout.Label("Other Values", EditorStyles.boldLabel);
		moduleBase.moneyValue = EditorGUILayout.IntField("Money Value", moduleBase.moneyValue);
	}

	void DrawActivationPart() {
		//Activation values
		GUILayout.Label("Activation values", EditorStyles.boldLabel);
		kv.moduleType = (ModuleValues.ModuleType)EditorGUILayout.EnumPopup("Module Type",kv.moduleType);
		kv.area = EditorGUILayout.FloatField("Activate Area", kv.area);
		kv.holdMin = EditorGUILayout.FloatField("Minimum Hold Duration", kv.holdMin);
		kv.holdMax = EditorGUILayout.FloatField("Maximum Hold Duration", kv.holdMax);

		GUILayout.Space(10);

		// Activation requirements
		GUILayout.Label("Activation Requirements", EditorStyles.boldLabel);
		if (GUILayout.Button("Add Activation Requirement")) {
			moduleBase.activations.Add(null);
		}

		for (int i = 0; i < moduleBase.activations.Count; i++) {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("X", GUILayout.Width(30))){
				moduleBase.activations.RemoveAt(i);
				i--;
				continue;
			}
			moduleBase.activations[i] = (ModuleActivation)EditorGUILayout.ObjectField(moduleBase.activations[i], typeof(ModuleActivation), false);
			GUILayout.EndHorizontal();
		}
	}

	void DrawEffectPart() {
		//Damage
		GUILayout.Label("Damage", EditorStyles.boldLabel);
		kv.damage = EditorGUILayout.IntField("Base Damage", kv.damage);
		kv.baseDamageScale = EditorGUILayout.FloatField("Additional Damage Scale", kv.baseDamageScale);
		kv.multihit = EditorGUILayout.Toggle("Multi Hit", kv.multihit);

		GUILayout.Space(10);

		//Projectile
		GUILayout.Label("Projectile", EditorStyles.boldLabel);
		kv.projectile = (Transform)EditorGUILayout.ObjectField("Projectile Object", kv.projectile, typeof(Transform),false);
		kv.projectileSpeed = EditorGUILayout.Vector2Field("Projectile Speed", kv.projectileSpeed);
		kv.projectileLifetime = EditorGUILayout.FloatField("Projectile Lifetime", kv.projectileLifetime);

		GUILayout.Space(10);

		// Effect creators
		GUILayout.Label("Module Effects", EditorStyles.boldLabel);
		if (GUILayout.Button("Add Effect")) {
			moduleBase.effects.Add(null);
		}

		for (int i = 0; i < moduleBase.effects.Count; i++) {
			GUILayout.BeginHorizontal();
			if (GUILayout.Button("X", GUILayout.Width(30))){
				moduleBase.effects.RemoveAt(i);
				i--;
			}
			moduleBase.effects[i] = (ModuleEffect)EditorGUILayout.ObjectField(moduleBase.effects[i], typeof(ModuleEffect), false);
			GUILayout.EndHorizontal();
		}
	}

	void SelectModule() {
		GUI.FocusControl(null);
		if (selModule == -1) {
			// Nothing selected
			moduleBase.ResetValues();
		}
		else {
			// Something selected
			Module ce = (Module)moduleLibrary.GetEntryByIndex(selModule);
			moduleBase.CopyValues(ce);
		}
	}

	void SaveSelectedModule() {
		Module ce = (Module)moduleLibrary.GetEntryByIndex(selModule);
		ce.CopyValues(moduleBase);
		Undo.RecordObject(ce, "Updated module");
		EditorUtility.SetDirty(ce);
	}

	void InstansiateModule() {
		GUI.FocusControl(null);
		if (moduleLibrary.ContainsID(uuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		Module c = Editor.CreateInstance<Module>();
		c.name = uuid;
		c.uuid = uuid;
		c.entryName = uuid;
		c.repColor = repColor;
		string path = "Assets/LibraryData/Modules/" + uuid + ".asset";

		AssetDatabase.CreateAsset(c, path);
		moduleLibrary.InsertEntry(c, 0);
		Undo.RecordObject(moduleLibrary, "Added module");
		EditorUtility.SetDirty(moduleLibrary);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		uuid = "";
		selModule = 0;
		SelectModule();
	}

	void DeleteModule() {
		GUI.FocusControl(null);
		Module c = (Module)moduleLibrary.GetEntryByIndex(selModule);
		string path = "Assets/LibraryData/Modules/" + c.uuid + ".asset";

		moduleLibrary.RemoveEntryByIndex(selModule);
		Undo.RecordObject(moduleLibrary, "Deleted module");
		EditorUtility.SetDirty(moduleLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		if (res) {
			Debug.Log("Removed module: " + c.uuid);
			selModule = -1;
		}
	}
}
