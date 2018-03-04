using UnityEngine;

[System.Serializable]
public class Frame {
	public BackgroundEntry background = null;
	public CharacterEntry[] characters = new CharacterEntry[Constants.DIALOGUE_PLAYERS_COUNT];
	public int[] poses = new int[Constants.DIALOGUE_PLAYERS_COUNT];
	public string talkingName = "";
	public int talkingIndex = -1;
	public string dialogueText = "";
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
	}
}