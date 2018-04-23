using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LibraryEntries/Dialogue")]
public class DialogueEntry : ScrObjLibraryEntry {

	public int size { get{ return frames.Count; } }
	public List<Color> participantColors = new List<Color>();

	public BattleEntry.NextLocation nextLocation = BattleEntry.NextLocation.OVERWORLD;
	public DialogueEntry dialogueEntry = null;
	public BattleEntry battleEntry = null;
	public bool changePosition = false;
	public Vector2 playerPosition = new Vector2();
	public Constants.OverworldArea nextArea = Constants.OverworldArea.DEFAULT;
	public Constants.CHAPTER TagEnum { 
		get { if (string.IsNullOrEmpty(tag)) return Constants.CHAPTER.DEFAULT;
				return (Constants.CHAPTER)System.Enum.Parse(typeof(Constants.CHAPTER),tag); } 
		set { tag = (value == Constants.CHAPTER.DEFAULT) ? "" : value.ToString(); } 
	}

	public List<Frame> frames = new List<Frame>();


	public override void ResetValues() {
		base.ResetValues();

		frames = new List<Frame>();
		participantColors = new List<Color>();

		frames.Add(new Frame());
		nextLocation = BattleEntry.NextLocation.OVERWORLD;
		dialogueEntry = null;
		battleEntry = null;
		changePosition = false;
		playerPosition = new Vector2();
		nextArea = Constants.OverworldArea.DEFAULT;
	}

	public override void CopyValues(ScrObjLibraryEntry other) {
		base.CopyValues(other);
		DialogueEntry de = (DialogueEntry)other;

		frames = de.frames;
		participantColors = de.participantColors;

		nextLocation = de.nextLocation;
		dialogueEntry = de.dialogueEntry;
		battleEntry = de.battleEntry;
		changePosition = de.changePosition;
		playerPosition = de.playerPosition;
		nextArea = de.nextArea;
	}

	public void InsertFrame(int index, Frame f) {
		frames.Insert(index, f);
	}

	public void RemoveFrame(int index) {
		frames.RemoveAt(index);
	}

	public GUIContent[] GenerateFrameRepresentation() {
		GUIContent[] list = new GUIContent[frames.Count];
		GUIContent content;
		string str;
		for (int i = 0; i < frames.Count; i++) {
			content = new GUIContent();
			str = frames[i].dialogueText.Split('\n')[0];
			content.text = i + ":  " + str.Substring(0,Mathf.Min(50,str.Length));
			list[i] = content;
		}
		return list;
	}
}
