using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class ItemEditorWindow {

	public ScrObjLibraryVariable itemLibrary;
	public ItemEquip itemValues;

	// Selection screen
	Rect selectRect = new Rect();
	Texture2D selectTex;
	Vector2 scrollPos;
	int selItem = -1;

	// Display screen
	Rect dispRect = new Rect();
	RectOffset dispOffset = new RectOffset();
	Texture2D dispTex;
	Vector2 dispScrollPos;
	StatsPercentModifier modifier;

	//Creation
	string uuid = "";
	Color repColor = new Color(0,0,0,1f);


	/// <summary>
	/// Constructor
	/// </summary>
	/// <param name="entries"></param>
	/// <param name="container"></param>
	public ItemEditorWindow(ScrObjLibraryVariable entries, ItemEquip container){
		itemLibrary = entries;
		itemValues = container;
		LoadLibrary();
	}

	void LoadLibrary() {

		Debug.Log("Loading item equip libraries...");

		itemLibrary.GenerateDictionary();

		Debug.Log("Finished loading item equip libraries");

		InitializeWindow();
	}

	public void InitializeWindow() {
		selectTex = new Texture2D(1, 1);
		selectTex.SetPixel(0, 0, new Color(0.8f, 0.8f, 0.8f));
		selectTex.Apply();

		dispTex = new Texture2D(1, 1);
		dispTex.SetPixel(0, 0, new Color(0.7f, 0.7f, 0.1f));
		dispTex.Apply();

		dispOffset.right = 10;

		itemValues.ResetValues();
	}


	public void DrawWindow() {

		GUILayout.BeginHorizontal();
		GUILayout.Label("Item Equip Editor", EditorStyles.boldLabel);
		if (selItem != -1) {
			if (GUILayout.Button("Save Item")){
				SaveSelectedItem();
			}
		}
		GUILayout.EndHorizontal();

		GenerateAreas();
		DrawBackgrounds();
		DrawEntryList();
		if (selItem != -1)
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

		int oldSelected = selItem;
		selItem = GUILayout.SelectionGrid(selItem, itemLibrary.GetRepresentations("",""),1);
		EditorGUILayout.EndScrollView();

		if (oldSelected != selItem)
			SelectItem();

		EditorGUIUtility.labelWidth = 110;
		GUILayout.Label("Create new item", EditorStyles.boldLabel);
		uuid = EditorGUILayout.TextField("Item Name", uuid);
		repColor = EditorGUILayout.ColorField("Display Color", repColor);
		if (GUILayout.Button("Create new")) {
			InstansiateItem();
		}
		if (GUILayout.Button("Delete item")) {
			DeleteItem();
		}

		GUILayout.EndArea();
	}

	void DrawDisplayWindow() {
		GUILayout.BeginArea(dispRect);
		dispScrollPos = GUILayout.BeginScrollView(dispScrollPos, GUILayout.Width(dispRect.width), 
							GUILayout.Height(dispRect.height-25));

		GUI.skin.textField.margin.right = 20;

		GUILayout.Label("Selected Item", EditorStyles.boldLabel);
		EditorGUILayout.SelectableLabel("UUID: " + itemValues.uuid);
		itemValues.repColor = EditorGUILayout.ColorField("List color", itemValues.repColor);

		GUILayout.Space(20);

		itemValues.entryName = EditorGUILayout.TextField("Name", itemValues.entryName);
		itemValues.icon = (Sprite)EditorGUILayout.ObjectField("Item Icon", itemValues.icon, typeof(Sprite),false);
		itemValues.tintColor = EditorGUILayout.ColorField("Item Tint Color", itemValues.tintColor);

		GUILayout.Space(5);

		itemValues.moneyValue = EditorGUILayout.IntField("Money Value", itemValues.moneyValue);

		GUILayout.Space(20);

		//Base Stats
EditorGUIUtility.labelWidth = 150;
		itemValues.healthModifier = EditorGUILayout.IntField("Health modifier", itemValues.healthModifier);
		itemValues.attackModifier = EditorGUILayout.IntField("Android Attack modifier", itemValues.attackModifier);
		itemValues.defenseModifier = EditorGUILayout.IntField("Android Defense modifier", itemValues.defenseModifier);
		itemValues.sAttackModifier = EditorGUILayout.IntField("Soldier Attack modifier", itemValues.sAttackModifier);
		itemValues.sDefenseModifier = EditorGUILayout.IntField("Soldier Defense modifier", itemValues.sDefenseModifier);
EditorGUIUtility.labelWidth = 100;

		//Percent Modifiers
		GUILayout.Label("Percent Modifiers", EditorStyles.boldLabel);

		if (GUILayout.Button("Add percent modifier")) {
			itemValues.percentModifiers.Add(new StatsPercentModifier());
		}

		for (int i = 0; i < itemValues.percentModifiers.Count; i++) {
			modifier = itemValues.percentModifiers[i];
			modifier.affectedStat = (StatsPercentModifier.Stat)EditorGUILayout.EnumPopup("Affected Stats",modifier.affectedStat);
			GUILayout.BeginHorizontal();
			modifier.percentDiff = EditorGUILayout.FloatField("Value (+/- %)", modifier.percentDiff);
			modifier.percentValue = 1f + (modifier.percentDiff * 0.01f);
			if (GUILayout.Button("X", GUILayout.Width(30))){
				itemValues.percentModifiers.RemoveAt(i);
				i--;
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(5);
		}

		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	void SelectItem() {
		GUI.FocusControl(null);
		if (selItem == -1) {
			// Nothing selected
			itemValues.ResetValues();
		}
		else {
			// Something selected
			ItemEquip ce = (ItemEquip)itemLibrary.GetEntryByIndex(selItem);
			itemValues.CopyValues(ce);
		}
	}

	void SaveSelectedItem() {
		ItemEquip ce = (ItemEquip)itemLibrary.GetEntryByIndex(selItem);
		ce.CopyValues(itemValues);
		Undo.RecordObject(ce, "Updated item");
		EditorUtility.SetDirty(ce);
	}

	void InstansiateItem() {
		GUI.FocusControl(null);
		if (itemLibrary.ContainsID(uuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		ItemEquip c = Editor.CreateInstance<ItemEquip>();
		c.name = uuid;
		c.uuid = uuid;
		c.entryName = uuid;
		c.repColor = repColor;
		string path = "Assets/LibraryData/Items/" + uuid + ".asset";

		AssetDatabase.CreateAsset(c, path);
		itemLibrary.InsertEntry(c, 0);
		Undo.RecordObject(itemLibrary, "Added item");
		EditorUtility.SetDirty(itemLibrary);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		uuid = "";
		selItem = 0;
		SelectItem();
	}

	void DeleteItem() {
		GUI.FocusControl(null);
		ItemEquip c = (ItemEquip)itemLibrary.GetEntryByIndex(selItem);
		string path = "Assets/LibraryData/Items/" + c.uuid + ".asset";

		itemLibrary.RemoveEntryByIndex(selItem);
		Undo.RecordObject(itemLibrary, "Deleted item");
		EditorUtility.SetDirty(itemLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		if (res) {
			Debug.Log("Removed item: " + c.uuid);
			selItem = -1;
		}
	}
}
