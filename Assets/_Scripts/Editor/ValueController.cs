using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class ValueController : EditorWindow {

	[Header("Other menus")]
	public CheatMenu cheatMenu;
	public DialogueSceneEditorWindow dialogueMenu;

	[Header("Area values")]
	public AreaInfoValues areaInfo;
	public IntVariable currentChapter;
	public AreaIntVariable currentArea;
	public AreaIntVariable playerArea;
	public IntVariable currentRoom;
	public BoolVariable forcePosition;

	[Header("Settings")]
	public FloatVariable musicVolume;
	public FloatVariable sfxVolume;

	private Constants.CHAPTER _currentChapter;
	private Constants.SCENE_INDEXES _currentArea;
	private Constants.SCENE_INDEXES _playerArea;
	private Constants.ROOMNUMBER _currentRoom;

	private int toolbarPos;
	private string[] toolbarStr = {"VALUES", "DIALOGUE", "SETTINGS", "CHEATS"};


	[MenuItem("Window/ValueController")]
	public static void ShowWindow() {
		GetWindow<ValueController>("Value Controller");
	}

	void OnEnable() {
		EditorSceneManager.sceneOpened += SceneOpenedCallback;
		InitializeWindow();
	}

	void OnDisable() {
		EditorSceneManager.sceneOpened -= SceneOpenedCallback;
	}

	/// <summary>
	/// Makes sure the window stays open when switching scenes.
	/// </summary>
	/// <param name="_scene"></param>
	/// <param name="_mode"></param>
	void SceneOpenedCallback(Scene _scene, OpenSceneMode _mode) {
		Debug.Log("SCENE LOADED");
		InitializeWindow();
	}

	/// <summary>
	/// Initializes all the window specific variables.
	/// </summary>
	void InitializeWindow() {
		cheatMenu = (CheatMenu)ScriptableObject.CreateInstance("CheatMenu");
		dialogueMenu = (DialogueSceneEditorWindow)ScriptableObject.CreateInstance("DialogueSceneEditorWindow");
		_currentChapter = (Constants.CHAPTER)currentChapter.value;
		_currentArea = (Constants.SCENE_INDEXES)currentArea.value;
		_playerArea = (Constants.SCENE_INDEXES)playerArea.value;
		_currentRoom = (Constants.ROOMNUMBER)currentRoom.value;
		Debug.Log("Init");
	}

	/// <summary>
	/// Renders the window.
	/// </summary>
	void OnGUI() {
		// Hotkeys();

		GUILayout.Label("Value Controller", EditorStyles.boldLabel);
		toolbarPos = GUILayout.Toolbar(toolbarPos, toolbarStr);
		GUILayout.Space(20);
		if (toolbarPos == 0) {
			DrawAreaValues();
			GUILayout.Space(20);
			if (GUILayout.Button("Start Game"))
				StartGame();
		}
		else if (toolbarPos == 1) {
			dialogueMenu.DrawGUI();
		}
		else if (toolbarPos == 2) {
			DrawSettings();
		}
		else if (toolbarPos == 3) {
			cheatMenu.DrawGUI();
		}
	}

	private void DrawSettings() {
		GUILayout.Label("Settings", EditorStyles.boldLabel);
		musicVolume.value = EditorGUILayout.Slider("Music Volume", musicVolume.value, 0, 1);
		sfxVolume.value = EditorGUILayout.Slider("Sfx Volume", sfxVolume.value, 0, 1);
	}

	void DrawAreaValues() {
		GUILayout.Label("Area values", EditorStyles.boldLabel);

		_currentChapter = (Constants.CHAPTER)EditorGUILayout.EnumPopup("Current Chapter", _currentChapter);
		_currentArea = (Constants.SCENE_INDEXES)EditorGUILayout.EnumPopup("Current Scene Index", _currentArea);
		_playerArea = (Constants.SCENE_INDEXES)EditorGUILayout.EnumPopup("Current OW Scene Index", _playerArea);
		_currentRoom = (Constants.ROOMNUMBER)EditorGUILayout.EnumPopup("Current Room", _currentRoom);
	}


	private void StartGame() {
		currentChapter.value = (int)_currentChapter;
		currentArea.value = (int)_currentArea;
		currentRoom.value = (int)_currentRoom;
		playerArea.value = (int)_playerArea;

		forcePosition.value = true;

		string filepath = "Assets/_Scenes/" + areaInfo.GetArea(currentArea.value, currentRoom.value).path + ".unity";
		EditorSceneManager.OpenScene(filepath);
		EditorApplication.isPlaying = true;
	}
}
