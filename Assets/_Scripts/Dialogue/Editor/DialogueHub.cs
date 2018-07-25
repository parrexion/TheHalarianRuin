using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class DialogueHub {

	public ScrObjLibraryVariable backgroundLibrary;
	public ScrObjLibraryVariable characterLibrary;
	public ScrObjLibraryVariable dialogueLibrary;

	public DialogueScene currentState;
	public DialogueEntry dialogueValues;
	private DialogueVisualContainer dvc;
	public bool closeTime = false;
	public string previousText = "";

	// Selected things
	public int selAction = -1;
	public int selDialogue = -1;

	//Creation
	public string dialogueUuid = "";
	Color repColor = new Color(0,0,0,0);


	public DialogueHub(ScrObjLibraryVariable bkg, ScrObjLibraryVariable chars, ScrObjLibraryVariable dialogue, DialogueEntry de) {
		backgroundLibrary = bkg;
		characterLibrary = chars;
		dialogueLibrary = dialogue;
		dialogueValues = de;
		currentState = GameObject.FindObjectOfType<DialogueScene>();
		dvc = GameObject.FindObjectOfType<DialogueVisualContainer>();
	}
	
	public void SelectDialogue() {
		// Nothing selected
		if (selDialogue == -1) {
			selAction = -1;
			dialogueValues.ResetValues();
		}
		else {
			// Something selected
			GUI.FocusControl(null);
			DialogueEntry de = (DialogueEntry)dialogueLibrary.GetEntryByIndex(selDialogue);
			dialogueValues.CopyValues(de);
			selAction = 0;
			UpdateToCurrentFrame();
		}
	}

	public void UpdateToCurrentFrame() {
		currentState.Reset();
		currentState.dialogueUuid.value = dialogueValues.uuid;
		previousText = "";
		DialogueActionData data;
		DialogueAction da;
		int stopPoint = selAction+1; //(dialogueValues.actions[selFrame].type == DActionType.MOVEMENT) ? selFrame : selFrame+1;
		for (int i = 0; i < stopPoint; i++) {
			data = dialogueValues.actions[i];
			da = DialogueAction.CreateAction(data.type);
			da.Act(currentState, data);
			if (data.type == DActionType.SET_TEXT)
				previousText = (!data.boolValue) ? previousText + data.text[1] : data.text[1];
		}

		currentState.backgroundChanged.Invoke();
		currentState.bkgMusicChanged.Invoke();
		currentState.characterChanged.Invoke();
		currentState.closeupChanged.Invoke();
		Debug.Log("TEXT:  " + currentState.dialogueText.value);

		Debug.Log("Updated to current state");
	}

	public void UpdateRealScene() {
		if (dialogueValues.actions[selAction].type == DActionType.SET_TEXT)
			DialogueAction.CreateAction(dialogueValues.actions[selAction].type).Act(currentState, dialogueValues.actions[selAction]);
		dvc = GameObject.FindObjectOfType<DialogueVisualContainer>();
		dvc.background.UpdateBackground();
		for (int i = 0; i < dvc.characters.Length; i++) {
			dvc.characters[i].UpdateCharacter();
		}
		dvc.closeup.UpdateCloseup();
		dvc.textBox.text = dvc.currentDialogueText.value;
		dvc.musicText.text = (dvc.currentMusic.value) ? dvc.currentMusic.value.name : "[NO MUSIC]";
	}

	public void SaveSelectedDialogue() {
		DialogueEntry de = (DialogueEntry)dialogueLibrary.GetEntryByIndex(selDialogue);
		de.CopyValues(dialogueValues);
		Undo.RecordObject(de, "Updated dialogue");
		EditorUtility.SetDirty(de);
	}

	public void InsertAction(DActionType type) {
		if (selAction < 2 || dialogueValues.actions[selAction].type == DActionType.END_SCENE)
			return;

		Debug.Log("Adding type : " + type);
		DialogueActionData data = new DialogueActionData();
		DialogueAction da = DialogueAction.CreateAction(type);
		da.FillData(data);

		dialogueValues.InsertAction(selAction+1,data);
		selAction++;

		SaveSelectedDialogue();
	}

	public void DeleteAction() {
		if (dialogueValues.actions.Count <= 4 || selAction < 3 || dialogueValues.actions[selAction].type == DActionType.END_SCENE)
			return;

		GUI.FocusControl(null);
		dialogueValues.RemoveAction(selAction);
		selAction = Mathf.Min(selAction, dialogueValues.actions.Count - 1);

		SaveSelectedDialogue();
	}

	public void InstansiateDialogue() {
		GUI.FocusControl(null);
		if (dialogueLibrary.ContainsID(dialogueUuid)) {
			Debug.Log("uuid already exists!");
			return;
		}
		DialogueEntry de = Editor.CreateInstance<DialogueEntry>();
		de.name = dialogueUuid;
		de.uuid = dialogueUuid;
		de.entryName = dialogueUuid;
		de.repColor = repColor;
		de.CreateBasicActions();
		string path = "Assets/LibraryData/Dialogues/" + dialogueUuid + ".asset";

		dialogueLibrary.InsertEntry(de,0);
		Undo.RecordObject(dialogueLibrary, "Added dialogue");
		EditorUtility.SetDirty(dialogueLibrary);
		AssetDatabase.CreateAsset(de, path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		dialogueUuid = "";
		selDialogue = 0;
		SelectDialogue();
	}

	public void DeleteDialogue() {
		GUI.FocusControl(null);
		DialogueEntry de = (DialogueEntry)dialogueLibrary.GetEntryByIndex(selDialogue);
		string path = "Assets/LibraryData/Dialogues/" + de.uuid + ".asset";

		dialogueLibrary.RemoveEntryByIndex(selDialogue);
		Undo.RecordObject(dialogueLibrary, "Deleted dialogue");
		EditorUtility.SetDirty(dialogueLibrary);
		bool res = AssetDatabase.MoveAssetToTrash(path);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();

		if (res) {
			Debug.Log("Removed dialogue: " + de.uuid);
			selDialogue = -1;
		}
	}
}
