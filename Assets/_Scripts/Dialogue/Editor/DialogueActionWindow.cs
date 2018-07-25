using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class DialogueActionWindow: EditorWindow {

	public struct SelectedPose
	{
		public int index;
		public int pose;
	}

	public BackgroundEntry noBackgroundSprite;
	public BackgroundEntry noCharacterSprite;
	public AreaIntVariable currentArea;
	public AreaIntVariable playerArea;
	public IntVariable currentAction;
	public BoolVariable overrideActionNumber;
	DialogueListWindow listWindow;
	DialogueHub hub;

	const int ACTION_WIDTH = 180;
	const int BUTTONS_WIDTH = 200;
	const int HEADER_HEIGHT = 60;
	Rect headerRect = new Rect();
	Rect buttonsRect = new Rect();
	Rect actionRect = new Rect();
	Rect editRect = new Rect();
	Texture2D actionBackground;
	Texture2D editBackground;

	string[] editModeStrings = new string[] { "TEXT", "CHARS", "BKG", "MUSIC", "SFX", "MOVE", "FLASH", "SHAKE", "DELAY" };
	string[] nextActionStrings = new string[] { "OVERWORLD", "DIALOGUE", "BATTLE" };
	string[] poseList = new string[] { "Normal", "Sad", "Happy", "Angry", "Dead", "Hmm", "Pleased", "Surprised", "Worried", "Grumpy" };
	string[] talkingPositionStrings = new string[] { "<", "x", "x", "x", "x", ">" };
	int[] talkingIndexConversion = new int[] { 1, 2, 3, 4, 0, 5 };
	int[] talkingIndexConversionReverse = new int[] { 4, 0, 1, 2, 3, 5 };

	//Other rects
	Rect bkgRect = new Rect(100,100,250,125);
	Rect closeupRect = new Rect(200,100,64,64);
	Rect[] characterRects = new Rect[4];
	Rect faceRect;

	// Selected things
	Vector2 actionScrollPos;
	string talkName = "";


	private void OnDestroy() {
		hub.closeTime = true;
	}

	/// <summary>
	/// Initializes the variables needed by the editor.
	/// </summary>
	public void InitializeWindow(DialogueHub hubWindow, DialogueListWindow mainWindow) {
		listWindow = mainWindow;
		hub = hubWindow;
		InitTextures();
	}

	public void InitTextures() {
		actionBackground = new Texture2D(1, 1);
		actionBackground.SetPixel(0, 0, new Color(0.75f, 0.6f, 0.25f));
		actionBackground.Apply();

		editBackground = new Texture2D(1, 1);
		editBackground.SetPixel(0, 0, new Color(0.25f, 0.6f, 0.75f));
		editBackground.Apply();
	}

	void OnSelectionChanged() {
		Repaint();
	}

	void OnGUI() {
		UpdateRects();
		DrawBackgrounds();
		if (hub.selDialogue == -1 || hub.selAction == -1)
			return;

		DrawHeader();
		DrawUpdateButtons();
		DrawActionList();
		switch (hub.dialogueValues.actions[hub.selAction].type)
		{
			case DActionType.START_SCENE: StartStuff(); break;
			case DActionType.SET_TEXT: DialogueTextStuff(); break;
			case DActionType.SET_CHARS: CharacterStuff(); break;
			case DActionType.SET_BKG: BackgroundStuff(); break;
			case DActionType.SET_MUSIC: MusicStuff(); break;
			case DActionType.PLAY_SFX: SfxStuff(); break;
			case DActionType.MOVEMENT: MovementStuff(); break;
			case DActionType.FLASH: EffectsStuff(); break;
			case DActionType.SHAKE: EffectsStuff(); break;
			case DActionType.DELAY: EffectsStuff(); break;
			case DActionType.END_SCENE: NextAreaStuff(); break;
		}
	}

	private void UpdateRects() {
		actionRect.x = 0;
		actionRect.y = HEADER_HEIGHT;
		actionRect.width = ACTION_WIDTH;
		actionRect.height = position.height - HEADER_HEIGHT;

		headerRect.x = 0;
		headerRect.y = 0;
		headerRect.width = position.width - BUTTONS_WIDTH;
		headerRect.height = HEADER_HEIGHT;

		buttonsRect.x = position.width - BUTTONS_WIDTH;
		buttonsRect.y = 0;
		buttonsRect.width = BUTTONS_WIDTH;
		buttonsRect.height = HEADER_HEIGHT;

		editRect.x = ACTION_WIDTH;
		editRect.y = HEADER_HEIGHT;
		editRect.width = position.width - ACTION_WIDTH;
		editRect.height = position.height - HEADER_HEIGHT;

		characterRects[0] = new Rect(0,HEADER_HEIGHT,editRect.width*0.25f,editRect.height);
		characterRects[1] = new Rect(editRect.width*0.25f,HEADER_HEIGHT,editRect.width*0.25f,editRect.height);
		characterRects[2] = new Rect(editRect.width*0.5f,HEADER_HEIGHT,editRect.width*0.25f,editRect.height);
		characterRects[3] = new Rect(editRect.width*0.75f,HEADER_HEIGHT,editRect.width*0.25f,editRect.height);

		faceRect = new Rect(editRect.width/15,96,64,64);
	}

	private void DrawBackgrounds() {
		GUI.DrawTexture(actionRect, actionBackground);
		GUI.DrawTexture(editRect, editBackground);
	}

	private void DrawHeader() {
		GUILayout.BeginArea(headerRect);
		// GUILayout.Label("Dialogue Action Window", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 100;
		
		//Dialogue Info
		EditorGUILayout.SelectableLabel("Selected Dialogue    UUID: " + hub.dialogueValues.uuid, EditorStyles.boldLabel, GUILayout.Height(20));
		if (hub.selAction != -1) {
			hub.dialogueValues.entryName = EditorGUILayout.TextField("Dialogue name", hub.dialogueValues.entryName, GUILayout.Width(headerRect.width - 30));
			hub.dialogueValues.TagEnum = (Constants.CHAPTER)EditorGUILayout.EnumPopup("Tag",hub.dialogueValues.TagEnum, GUILayout.Width(headerRect.width - 30));
		}
		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the frame adjustment button.
	/// </summary>
	private void DrawUpdateButtons() {
		GUILayout.BeginArea(buttonsRect);

		GUILayout.Space(10);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button("Update\nScene", GUILayout.Height(buttonsRect.height-10))) {
			hub.UpdateRealScene();
		}
		if (GUILayout.Button("Update\nand play", GUILayout.Height(buttonsRect.height-10))) {
			hub.UpdateRealScene();
			currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
			playerArea.value = (int)Constants.SCENE_INDEXES.TEST_SCENE;
			currentAction.value = hub.selAction;
			overrideActionNumber.value = true;
			Scene currentScene = SceneManager.GetActiveScene();
			if (currentScene.name != "Dialogue")
				EditorSceneManager.OpenScene("Assets/_Scenes/Dialogue.unity");
			EditorApplication.isPlaying = true;
		}
		GUILayout.EndHorizontal();

		GUILayout.EndArea();
	}

	private void DrawActionList() {
		GUILayout.BeginArea(actionRect);
		GUILayout.Label("Add new action", EditorStyles.boldLabel);
		actionScrollPos = GUILayout.BeginScrollView(actionScrollPos, GUILayout.Width(actionRect.width), 
					GUILayout.Height(actionRect.height - 80));

		for (int i = 0; i < editModeStrings.Length; i++) {
			if (GUILayout.Button(editModeStrings[i], GUILayout.Height(25))){
				hub.InsertAction((DActionType)(i+2));
				listWindow.Repaint();
			}
		}

		GUILayout.EndScrollView();

		GUILayout.Space(20);
		GUIStyle style = new GUIStyle(GUI.skin.button);
		style.normal.textColor = Color.red;
		if (GUILayout.Button("DELETE ACTION", style, GUILayout.Height(30))){
			hub.DeleteAction();
			listWindow.Repaint();
		}
		GUILayout.EndArea();
	}


	////////////////////
	/// ACTION STUFF ///
	////////////////////


	/// <summary>
	/// Renders the setup options for the start action.
	/// </summary>
	void StartStuff() {
		GUILayout.BeginArea(editRect);
		GUILayout.Label("Start stuff", EditorStyles.boldLabel);
		GUILayout.Space(10);

		//Background
		GUILayout.Label("Set background", EditorStyles.boldLabel);
		hub.dialogueValues.actions[hub.selAction].entries[0] = (BackgroundEntry)EditorGUILayout.ObjectField("Background", hub.dialogueValues.actions[hub.selAction].entries[0], typeof(BackgroundEntry),false);
		if (hub.dialogueValues.actions[hub.selAction].entries[0] != null){
			GUI.DrawTexture(bkgRect, ((BackgroundEntry)hub.dialogueValues.actions[hub.selAction].entries[0]).sprite.texture);
		}
		else
			GUI.DrawTexture(bkgRect, noBackgroundSprite.sprite.texture);

		//Music
		EditorGUIUtility.labelWidth = 130;
		GUILayout.Label("Background Music", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 100;
		hub.dialogueValues.actions[hub.selAction].entries[1] = (MusicEntry)EditorGUILayout.ObjectField("Bkg Music", hub.dialogueValues.actions[hub.selAction].entries[1], typeof(MusicEntry), false);
		
		//Characters

		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the dialogue text area.
	/// </summary>
	void DialogueTextStuff() {
		EditorGUIUtility.labelWidth = 200;
		int talkingIndex = hub.dialogueValues.actions[hub.selAction].values[0];
		if (hub.dialogueValues.actions[hub.selAction].values[0] == 4)
			talkName = hub.dialogueValues.actions[hub.selAction].text[0];

		GUILayout.BeginArea(editRect);

		hub.dialogueValues.actions[hub.selAction].boolValue = GUILayout.Toggle(hub.dialogueValues.actions[hub.selAction].boolValue, "Clear prev. text");

		//Text box
		EditorStyles.textField.wordWrap = true;
		GUILayout.Label("Set Dialogue Text", EditorStyles.boldLabel);
		GUILayout.BeginHorizontal();
		hub.dialogueValues.actions[hub.selAction].text[1] = EditorGUILayout.TextArea(hub.dialogueValues.actions[hub.selAction].text[1], GUILayout.Width(300), GUILayout.Height(48));
		EditorGUI.BeginDisabledGroup(true);
		string str = (!hub.dialogueValues.actions[hub.selAction].boolValue) ? hub.previousText : hub.dialogueValues.actions[hub.selAction].text[1];
		EditorGUILayout.TextArea(str, GUILayout.Width(300), GUILayout.Height(48));
		EditorGUI.EndDisabledGroup();
		GUILayout.EndHorizontal();

		//Closeup
		GUILayout.Label("Speaker's Name: ", EditorStyles.boldLabel);
		if (talkingIndex == -1 || talkingIndex == 4) {
			GUILayout.Label(hub.dialogueValues.actions[hub.selAction].text[0], EditorStyles.boldLabel, GUILayout.Width(80));
		}
		else if (hub.currentState.characters[talkingIndex].value != null) {
			GUILayout.Label(hub.dialogueValues.actions[hub.selAction].text[0], EditorStyles.boldLabel, GUILayout.Width(80));

			GUI.DrawTexture(closeupRect, ((CharacterEntry)hub.currentState.characters[talkingIndex].value).defaultColor.texture);
			GUILayout.BeginArea(closeupRect);
			GUILayout.Label(((CharacterEntry)hub.currentState.characters[talkingIndex].value).poses[hub.currentState.poses[talkingIndex].value].texture);
			GUILayout.EndArea();
		}

		GUILayout.Space(65);

		//Talking stuff

		GUIContent[] filteredList = new GUIContent[5];
		GUIContent content;
		for (int i = 0; i < Constants.DIALOGUE_PLAYERS_COUNT; i++) {
			content = new GUIContent();
			filteredList[i] = content;
			content.text = "Talking";
			CharacterEntry ce = (CharacterEntry)hub.currentState.characters[i].value;
			if (ce == null)
				continue;

			Texture tex = ce.GenerateRepresentation().image;
			content.image = tex;
		}
		content = new GUIContent();
		content.text = "??? is Talking";
		filteredList[4] = content;

		talkingIndex = GUILayout.SelectionGrid(hub.dialogueValues.actions[hub.selAction].values[0], filteredList, 5, GUILayout.Height(30));
		if (GUILayout.Button("No one is talking")) {
			talkingIndex = -1;
		}

		if (talkingIndex == -1) {
			hub.dialogueValues.actions[hub.selAction].text[0] = "";
		}
		else if (talkingIndex == 4) {
			talkName = EditorGUILayout.TextField("Other talking person", talkName);
			hub.dialogueValues.actions[hub.selAction].text[0] = talkName;
		}
		else if (hub.currentState.characters[talkingIndex].value != null) {
			hub.dialogueValues.actions[hub.selAction].text[0] = hub.currentState.characters[talkingIndex].value.entryName;
		}
		else {
			talkingIndex = -1;
			hub.dialogueValues.actions[hub.selAction].text[0] = "";
		}

		hub.dialogueValues.actions[hub.selAction].values[0] = talkingIndex;

		GUILayout.Space(25);
		
		//Other options
		GUILayout.Label("Other text options", EditorStyles.boldLabel);
		hub.dialogueValues.actions[hub.selAction].autoContinue = EditorGUILayout.Toggle("Auto Continue", hub.dialogueValues.actions[hub.selAction].autoContinue);

		EditorGUIUtility.labelWidth = 100;
		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the characters and talking part.
	/// </summary>
	void CharacterStuff() {
		GUILayout.BeginArea(editRect);
		float fieldWidth = editRect.width * 0.25f;
		EditorGUIUtility.labelWidth = fieldWidth;
		
		GUILayout.Label("Set character and poses", EditorStyles.boldLabel);
		for (int j = 0; j < 4; j++) {
			GUILayout.BeginArea(characterRects[j]);
			if (hub.dialogueValues.actions[hub.selAction].entries[j] != null)
				GUILayout.Label(hub.dialogueValues.actions[hub.selAction].entries[j].entryName, EditorStyles.boldLabel);
			else
				GUILayout.Label("", EditorStyles.boldLabel);

			GUILayout.Label("Character " + j);

			hub.dialogueValues.actions[hub.selAction].entries[j] = (CharacterEntry)EditorGUILayout.ObjectField("", hub.dialogueValues.actions[hub.selAction].entries[j], typeof(CharacterEntry),false, GUILayout.Width(fieldWidth-8));
			if (hub.dialogueValues.actions[hub.selAction].entries[j] == null){
				hub.dialogueValues.actions[hub.selAction].values[j] = -1;
				GUILayout.EndArea();
				continue;
			}

			if (hub.dialogueValues.actions[hub.selAction].values[j] == -1)
					hub.dialogueValues.actions[hub.selAction].values[j] = 0;
			
			if (GUILayout.Button(poseList[hub.dialogueValues.actions[hub.selAction].values[j]], GUILayout.Width(fieldWidth-8))) {
				GenericMenu menu = new GenericMenu();
				SelectedPose selPose = new SelectedPose();
				selPose.index = j;
				for (int p = 0; p < poseList.Length; p++) {
					selPose.pose = p;
					menu.AddItem(new GUIContent(poseList[p]), (p == hub.dialogueValues.actions[hub.selAction].values[j]), SetPose, selPose);
				}
				menu.ShowAsContext();
			}

			GUILayout.Space(20);

			if (hub.dialogueValues.actions[hub.selAction].entries[j] == null) {
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label(noCharacterSprite.sprite.texture);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}
			else {
				GUILayout.BeginHorizontal();
				GUI.DrawTexture(faceRect, ((CharacterEntry)hub.dialogueValues.actions[hub.selAction].entries[j]).defaultColor.texture);
				GUILayout.FlexibleSpace();
				GUILayout.Label(((CharacterEntry)hub.dialogueValues.actions[hub.selAction].entries[j]).poses[hub.dialogueValues.actions[hub.selAction].values[j]].texture);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
			}

			GUILayout.EndArea();
		}

		GUILayout.EndArea();
		EditorGUIUtility.labelWidth = 100;
	}

	/// <summary>
	/// Sets the pose of the character.
	/// </summary>
	/// <param name="selectedPose"></param>
	void SetPose(object selectedPose) {
		SelectedPose pose = (SelectedPose)selectedPose;
		hub.dialogueValues.actions[hub.selAction].values[pose.index] = pose.pose;
	}

	/// <summary>
	/// Renders the header part with the background selection.
	/// </summary>
	void BackgroundStuff() {
		GUILayout.BeginArea(editRect);
		GUILayout.Label("Set background", EditorStyles.boldLabel);

		hub.dialogueValues.actions[hub.selAction].entries[0] = (BackgroundEntry)EditorGUILayout.ObjectField("Background", hub.dialogueValues.actions[hub.selAction].entries[0], typeof(BackgroundEntry),false);
		if (hub.dialogueValues.actions[hub.selAction].entries[0] != null){
			GUI.DrawTexture(bkgRect, ((BackgroundEntry)hub.dialogueValues.actions[hub.selAction].entries[0]).sprite.texture);
		}
		else
			GUI.DrawTexture(bkgRect, noBackgroundSprite.sprite.texture);

		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the sounds used in the dialogues.
	/// </summary>
	void MusicStuff() {
		GUILayout.BeginArea(editRect);
		EditorGUIUtility.labelWidth = 130;
		GUILayout.Label("Background Music", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 100;
		hub.dialogueValues.actions[hub.selAction].entries[0] = (MusicEntry)EditorGUILayout.ObjectField("Bkg Music", hub.dialogueValues.actions[hub.selAction].entries[0], typeof(MusicEntry), false);
		
		// //Show selected music
		// int lastIndex = hub.selFrame;
		// while (lastIndex >= 0) {
		// 	if (dialogueValues.frames[lastIndex].bkgMusic != null)
		// 		break;
		// 	lastIndex--;
		// }
		// if (lastIndex >= 0)
		// 	GUILayout.Label("Selected Music: " + dialogueValues.frames[lastIndex].bkgMusic.name);
		// else
		// 	GUILayout.Label("Selected Music: NONE");

		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the sounds used in the dialogues.
	/// </summary>
	void SfxStuff() {
		GUILayout.BeginArea(editRect);
		EditorGUIUtility.labelWidth = 130;
		GUILayout.Label("Play Sound Effect", EditorStyles.boldLabel);
		EditorGUIUtility.labelWidth = 100;
	
		hub.dialogueValues.actions[hub.selAction].entries[0] = (SfxEntry)EditorGUILayout.ObjectField("Sound effect", hub.dialogueValues.actions[hub.selAction].entries[0], typeof(SfxEntry), false);

		GUILayout.Label("Add sfx delay", EditorStyles.boldLabel);
		hub.dialogueValues.actions[hub.selAction].values[0] = EditorGUILayout.IntField("Sfx delay (ms)",hub.dialogueValues.actions[hub.selAction].values[0]);

		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the character movements.
	/// </summary>
	void MovementStuff() {
		GUILayout.BeginArea(editRect);
		EditorGUIUtility.labelWidth = 150;
		GUILayout.Label("Dialogue Movements", EditorStyles.boldLabel);

		hub.dialogueValues.actions[hub.selAction].values[0] = EditorGUILayout.IntField("Animation speed (ms)",hub.dialogueValues.actions[hub.selAction].values[0]);

		GUILayout.Space(20);

		if (GUILayout.Button("Add movement")) {
			hub.dialogueValues.actions[hub.selAction].values.Add(0);
			hub.dialogueValues.actions[hub.selAction].values.Add(0);
		}
			GUILayout.BeginHorizontal();
			GUILayout.Label("Start position");
			GUILayout.Label("End position");
			GUILayout.EndHorizontal();

		for (int i = 1; i < hub.dialogueValues.actions[hub.selAction].values.Count; i+=2) {
			GUILayout.BeginHorizontal();
			int index = talkingIndexConversion[hub.dialogueValues.actions[hub.selAction].values[i]];
			index = GUILayout.SelectionGrid(index, talkingPositionStrings, 6);
			hub.dialogueValues.actions[hub.selAction].values[i] = talkingIndexConversionReverse[index];
			GUILayout.Label("    ->", GUILayout.Width(50));
			index = talkingIndexConversion[hub.dialogueValues.actions[hub.selAction].values[i+1]];
			index = GUILayout.SelectionGrid(index, talkingPositionStrings, 6);
			hub.dialogueValues.actions[hub.selAction].values[i+1] = talkingIndexConversionReverse[index];
			GUIStyle style = new GUIStyle(GUI.skin.button);
			GUILayout.Space(20);
			style.normal.textColor = Color.red;
			if (GUILayout.Button("X", style, GUILayout.Width(30))){
				GUI.FocusControl(null);
				hub.dialogueValues.actions[hub.selAction].values.RemoveAt(i);
				hub.dialogueValues.actions[hub.selAction].values.RemoveAt(i);
				i-=2;
			}
			GUILayout.EndHorizontal();
		}

		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the different effects available.
	/// </summary>
	void EffectsStuff() {
		GUILayout.BeginArea(editRect);
		EditorGUIUtility.labelWidth = 150;
		// Flashes
		if (hub.dialogueValues.actions[hub.selAction].type == DActionType.FLASH) {
			GUILayout.Label("Screen Flash", EditorStyles.boldLabel);
			hub.dialogueValues.actions[hub.selAction].values[0] = EditorGUILayout.IntField("Flash startup (ms)",hub.dialogueValues.actions[hub.selAction].values[0]);
			hub.dialogueValues.actions[hub.selAction].values[1] = EditorGUILayout.IntField("Flash fade (ms)",hub.dialogueValues.actions[hub.selAction].values[1]);
		}
		// Shakes
		else if (hub.dialogueValues.actions[hub.selAction].type == DActionType.SHAKE) {
			GUILayout.Label("Screen Shake", EditorStyles.boldLabel);
			hub.dialogueValues.actions[hub.selAction].values[0] = EditorGUILayout.IntField("Shake duration (ms)",hub.dialogueValues.actions[hub.selAction].values[0]);
			hub.dialogueValues.actions[hub.selAction].values[1] = EditorGUILayout.IntField("After Shake delay (ms)",hub.dialogueValues.actions[hub.selAction].values[1]);
		}
		// Delays
		else if (hub.dialogueValues.actions[hub.selAction].type == DActionType.DELAY) {
			GUILayout.Label("Dialogue delay", EditorStyles.boldLabel);
			hub.dialogueValues.actions[hub.selAction].values[0] = EditorGUILayout.IntField("Delay duration (ms)",hub.dialogueValues.actions[hub.selAction].values[0]);
		}

		EditorGUIUtility.labelWidth = 100;

		GUILayout.EndArea();
	}

	/// <summary>
	/// Renders the next area values.
	/// </summary>
	void NextAreaStuff() {
		GUILayout.BeginArea(editRect);
		GUILayout.Label("Next Action", EditorStyles.boldLabel);
		hub.dialogueValues.nextLocation = (BattleEntry.NextLocation)GUILayout.Toolbar((int)hub.dialogueValues.nextLocation, nextActionStrings);
		GUILayout.Space(10);

		switch (hub.dialogueValues.nextLocation)
		{
			case BattleEntry.NextLocation.OVERWORLD:
				EditorGUIUtility.labelWidth = 150;
				hub.dialogueValues.changePosition = EditorGUILayout.Toggle("Change player position", hub.dialogueValues.changePosition, GUILayout.Width(190));
				if (hub.dialogueValues.changePosition) {
					GUILayout.Space(10);
					EditorGUIUtility.labelWidth = 60;
					hub.dialogueValues.nextArea = (Constants.OverworldArea)EditorGUILayout.EnumPopup("Area", hub.dialogueValues.nextArea);
					GUILayout.Space(10);
					EditorGUIUtility.labelWidth = 100;
					hub.dialogueValues.playerPosition = EditorGUILayout.Vector2Field("Player Position", hub.dialogueValues.playerPosition, GUILayout.Width(editRect.width-8));
				}
				else {
					EditorGUIUtility.labelWidth = 100;
				}
				break;
			case BattleEntry.NextLocation.DIALOGUE:
				hub.dialogueValues.dialogueEntry = (DialogueEntry)EditorGUILayout.ObjectField("Next dialogue", hub.dialogueValues.dialogueEntry, typeof(DialogueEntry),false);
				break;
			case BattleEntry.NextLocation.BATTLE:
				hub.dialogueValues.battleEntry = (BattleEntry)EditorGUILayout.ObjectField("Next battle", hub.dialogueValues.battleEntry, typeof(BattleEntry),false);
				break;
		}
		GUILayout.EndArea();
	}
}
