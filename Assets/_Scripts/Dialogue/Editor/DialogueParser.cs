using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class DialogueParser {

	public ScrObjLibraryVariable backgroudLibrary;
	public ScrObjLibraryVariable characterLibrary;
	public ScrObjLibraryVariable dialogueLibrary;

	public DialogueCollection dialogues;
	public DialogueEntry entry;


	// Use this for initialization
	public void Generate() {
		string file = Path.Combine(Application.streamingAssetsPath,"test.json");
		// string file = "Assets/StreamingAssets/test.json";
		dialogues = new DialogueCollection(1);
		entry.ResetValues();

		LoadDialogue(file);
		GenerateDialogueEntry();
	}

	void LoadDialogue(string filename) {

		if (File.Exists(filename)) {
			string dataAsJson = File.ReadAllText(filename);
			dialogues = JsonUtility.FromJson<DialogueCollection>(dataAsJson);

			Debug.Log("Number of dialogues: "+dialogues.dialogues.Length);
		}
		else {
			Debug.LogError("Could not open file: "+filename);
		}
	}

	void GenerateDialogueEntry() {
		Dialogue d = dialogues.dialogues[0];

		entry.entryName = d.name;

		Frame f;
		OldFrame df;
		for (int i = 0; i < d.frames.Count; i++) {
			df = d.frames[i];
			f = new Frame();
			f.background = (BackgroundEntry)backgroudLibrary.GetEntryByIndex(df.background);
			for (int j = 0; j < Constants.DIALOGUE_PLAYERS_COUNT; j++) {
				f.characters[j] = (CharacterEntry)characterLibrary.GetEntryByIndex(df.currentCharacters[j]);
				f.poses[j] = df.currentPoses[j];
			}
			f.talkingName = df.characterName;
			f.dialogueText = df.dialogueText;
			f.talkingIndex = df.talkingPosition;
			entry.frames.Add(f);
		}

		Undo.RecordObject(entry, "Parsed dialogue");
		EditorUtility.SetDirty(entry);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}

	// public void UpdateDialogueEntry(string uuid) {
	// 	dialogueLibrary.GenerateDictionary();
	// 	entry = (DialogueEntry)dialogueLibrary.GetEntry(uuid);
	// 	Frame f;

	// 	for (int i = 0; i < entry.frames.Count; i++) {
	// 		f = entry.frames[i];
	// 		f.talkingChar = (f.talkingIndex < 0 || f.talkingIndex > 3) ? null : f.characters[f.talkingIndex];
	// 	}
	// 	Debug.Log("Updated entry");

	// 	Undo.RecordObject(entry, "Updated dialogue entry");
	// 	EditorUtility.SetDirty(entry);
	// 	AssetDatabase.SaveAssets();
	// 	AssetDatabase.Refresh();
	// }
}
