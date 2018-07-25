using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DActionType {START_SCENE,END_SCENE,SET_TEXT,SET_CHARS,SET_BKG,SET_MUSIC,PLAY_SFX,MOVEMENT,FLASH, SHAKE, DELAY};

[System.Serializable]
public class DialogueActionData {

	public bool autoContinue = true;
	public bool useDelay = false;
	public DActionType type;
	public List<ScrObjLibraryEntry> entries = new List<ScrObjLibraryEntry>();
	public List<int> values = new List<int>();
	public List<string> text = new List<string>();
	public bool boolValue;


	public void CopyValues(DialogueActionData other) {
		autoContinue = other.autoContinue;
		useDelay = other.useDelay;
		type = other.type;
		entries = other.entries;
		values = other.values;
		text = other.text;
		boolValue = other.boolValue;
	}
}