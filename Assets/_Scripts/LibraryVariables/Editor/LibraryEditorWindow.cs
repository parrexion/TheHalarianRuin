using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class LibraryEditorWindow : EditorWindow {

	private enum State { BATTLE = 0, CHARACTER = 1, ENEMY = 2, BACKGROUND = 3, ITEMEQUIP = 4, MODULE = 5, MUSIC = 6 }

	// Header
	Rect headerRect = new Rect();
	Texture2D headerTex;

	public IntVariable currentWindow;

	public BattleEditorWindow battleEditor;
	public ScrObjLibraryVariable battleLibrary;
	public BattleEntry battleContainer;

	public CharacterEditorWindow characterEditor;
	public ScrObjLibraryVariable characterLibrary;
	public CharacterEntry charContainer;
	public SpriteListVariable poseList;

	public EnemyEditorWindow enemyEditor;
	public ScrObjLibraryVariable enemyLibrary;
	public EnemyEntry enemyContainer;

	public BackgroundEditorWindow backgroundEditor;
	public ScrObjLibraryVariable backgroundLibrary;
	public BackgroundEntry backgroundContainer;

	public ItemEditorWindow itemEditor;
	public ScrObjLibraryVariable itemLibrary;
	public ItemEquip itemContainer;

	public ModuleEditorWindow moduleEditor;
	public ScrObjLibraryVariable moduleLibrary;
	public Module moduleContainer;

	public MusicEditorWindow musicEditor;
	public ScrObjLibraryVariable musicLibrary;
	public MusicEntry musicContainer;

	private string[] toolbarStrings = new string[] {"Battles", "Characters", "Enemies", "Background", "Items", "Module", "Music"};


	[MenuItem("Window/LibraryEditor")]
	public static void ShowWindow() {
		GetWindow<LibraryEditorWindow>("Library Editor");
	}

	void OnEnable() {
		EditorSceneManager.sceneOpened += SceneOpenedCallback;
		LoadLibraries();
	}

	void OnDisable() {
		EditorSceneManager.sceneOpened -= SceneOpenedCallback;
	}

	/// <summary>
	/// Renders the selected window.
	/// </summary>
	void OnGUI() {
		DrawHeader();
		
		switch ((State)currentWindow.value)
		{
			case State.BATTLE:
				battleEditor.DrawWindow();
				break;
			case State.CHARACTER:
				characterEditor.DrawWindow();
				break;
			case State.ENEMY:
				enemyEditor.DrawWindow();
				break;
			case State.BACKGROUND:
				backgroundEditor.DrawWindow();
				break;
			case State.ITEMEQUIP:
				itemEditor.DrawWindow();
				break;
			case State.MODULE:
				moduleEditor.DrawWindow();
				break;
			case State.MUSIC:
				musicEditor.DrawWindow();
				break;
		}
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
	/// Loads all the libraries for the editors.
	/// </summary>
	void LoadLibraries() {
		battleEditor = new BattleEditorWindow(battleLibrary, battleContainer);
		characterEditor = new CharacterEditorWindow(characterLibrary, charContainer, poseList);
		enemyEditor = new EnemyEditorWindow(enemyLibrary, enemyContainer);
		backgroundEditor = new BackgroundEditorWindow(backgroundLibrary, backgroundContainer);
		itemEditor = new ItemEditorWindow(itemLibrary, itemContainer);
		moduleEditor = new ModuleEditorWindow(moduleLibrary, moduleContainer);
		musicEditor = new MusicEditorWindow(musicLibrary, musicContainer);

		InitializeWindow();
	}

	/// <summary>
	/// Initializes all the window specific variables.
	/// </summary>
	void InitializeWindow() {
		headerTex = new Texture2D(1, 1);
		headerTex.SetPixel(0, 0, new Color(0.5f, 0.2f, 0.8f));
		headerTex.Apply();

		battleEditor.InitializeWindow();
		characterEditor.InitializeWindow();
		enemyEditor.InitializeWindow();
		backgroundEditor.InitializeWindow();
		itemEditor.InitializeWindow();
		moduleEditor.InitializeWindow();
		musicEditor.InitializeWindow();
	}

	/// <summary>
	/// Draws the header for the editor.
	/// </summary>
	void DrawHeader() {
		headerRect.x = 0;
		headerRect.y = 0;
		headerRect.width = Screen.width;
		headerRect.height = 50;
		GUI.DrawTexture(headerRect, headerTex);

		currentWindow.value = GUILayout.Toolbar(currentWindow.value, toolbarStrings);
	}
}
