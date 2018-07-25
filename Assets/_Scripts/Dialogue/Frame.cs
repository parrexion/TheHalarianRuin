using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Frame {
	public BackgroundEntry background = null;
	public CharacterEntry[] characters = new CharacterEntry[Constants.DIALOGUE_PLAYERS_COUNT];
	public int[] poses = new int[Constants.DIALOGUE_PLAYERS_COUNT];
	public string talkingName = "";
	public string dialogueText = "";
	public int talkingIndex = -1;
	public CharacterEntry talkingChar { get {
			if (talkingIndex == -1 || talkingIndex == 4)
				return null;
			return characters[talkingIndex];
		} }
	public int talkingPose { get {
			if (talkingIndex == -1 || talkingIndex == 4)
				return -1;
			return poses[talkingIndex];
		} }
	public MusicEntry bkgMusic = null;
	public List<DialogueMoveTuple> movements = new List<DialogueMoveTuple>();


	public void CopyValues(Frame other) {
		background = other.background;
		characters = new CharacterEntry[Constants.DIALOGUE_PLAYERS_COUNT];
		poses = new int[Constants.DIALOGUE_PLAYERS_COUNT];
		for (int i = 0; i < Constants.DIALOGUE_PLAYERS_COUNT; i++) {
			characters[i] = other.characters[i];
			poses[i] = other.poses[i];
		}
		talkingIndex = other.talkingIndex;
		talkingName = other.talkingName;
		dialogueText = other.dialogueText;
		bkgMusic = other.bkgMusic;
		movements = new List<DialogueMoveTuple>();
		for (int i = 0; i < other.movements.Count; i++) {
			movements.Add(other.movements[i]);
		}
	}


}

public class DialogueMoveTuple {
	public int start;
	public int end;
}