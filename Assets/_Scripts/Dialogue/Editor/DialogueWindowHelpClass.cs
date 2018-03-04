using UnityEngine;

public class DialogueWindowHelpClass {

	public Texture2D headerBackground;
	public Texture2D backgroundChars;
	public Texture2D backgroundChars2;
	public Texture2D talkingBackground;
	public Texture2D dialogueBackground;
	public Texture2D frameBackground;
	public Texture2D nextBackground;
	public Texture2D soundBackground;
	public Texture2D rightBackground;

	public Color headerColor = new Color(0.25f,0.5f,0.75f);
	public Color charactersColor = new Color(0.35f, 0.75f, 0.35f);
	public Color charactersColor2 = new Color(0.15f, 0.55f, 0.15f);
	public Color talkingColor = new Color(0.7f, 0.7f, 0.7f);
	public Color dialogueColor = new Color(0.5f, 0.5f, 0.5f);
	public Color frameColor = new Color(0.6f, 0.4f, 0.3f);
	public Color nextColor = new Color(0.4f, 0.3f, 0.6f);
	public Color soundColor = new Color(0.8f, 0.8f, 0.2f);
	public Color rightColor = new Color(0.4f, 0.6f, 0f);

	public Rect headerRect = new Rect();
	public Rect[] rectChar = new Rect[5];
	public Rect talkingRect = new Rect();
	public Rect dialogueRect = new Rect();
	public Rect frameRect = new Rect();
	public Rect nextRect = new Rect();
	public Rect soundRect = new Rect();
	public Rect rightRect = new Rect();


	public void InitTextures() {
		headerBackground = new Texture2D(1, 1);
		headerBackground.SetPixel(0, 0, headerColor);
		headerBackground.Apply();

		backgroundChars = new Texture2D(1, 1);
		backgroundChars.SetPixel(0, 0, charactersColor);
		backgroundChars.Apply();

		backgroundChars2 = new Texture2D(1, 1);
		backgroundChars2.SetPixel(0, 0, charactersColor2);
		backgroundChars2.Apply();

		backgroundChars2 = new Texture2D(1, 1);
		backgroundChars2.SetPixel(0, 0, charactersColor2);
		backgroundChars2.Apply();

		talkingBackground = new Texture2D(1, 1);
		talkingBackground.SetPixel(0, 0, talkingColor);
		talkingBackground.Apply();

		dialogueBackground = new Texture2D(1, 1);
		dialogueBackground.SetPixel(0, 0, dialogueColor);
		dialogueBackground.Apply();

		frameBackground = new Texture2D(1, 1);
		frameBackground.SetPixel(0, 0, frameColor);
		frameBackground.Apply();

		nextBackground = new Texture2D(1, 1);
		nextBackground.SetPixel(0, 0, nextColor);
		nextBackground.Apply();

		soundBackground = new Texture2D(1, 1);
		soundBackground.SetPixel(0, 0, soundColor);
		soundBackground.Apply();

		rightBackground = new Texture2D(1, 1);
		rightBackground.SetPixel(0, 0, rightColor);
		rightBackground.Apply();
	}

	public void GenerateAreas() {
		int topPartHeight = 80;
		int characterWidth = 140;
		int characterHeight = 148;
		int talkingHeight = 44;
		int dialoguePartWidth = 320;
		int dialoguePartHeight = 140;
		int bottomPartHeight = 168;
		int soundPartHeight = 80;
		int screenStep = characterWidth; //(Screen.width - rightPartWidth) / 5f;

		headerRect.x = 0;
		headerRect.y = 0;
		headerRect.width = characterWidth * 5;
		headerRect.height = topPartHeight;

		rectChar[0].x = 0;
		rectChar[0].y = topPartHeight;
		rectChar[0].width = screenStep;
		rectChar[0].height = characterHeight;

		rectChar[1].x = screenStep;
		rectChar[1].y = topPartHeight;
		rectChar[1].width = screenStep;
		rectChar[1].height = characterHeight;

		rectChar[2].x = screenStep *2;
		rectChar[2].y = topPartHeight;
		rectChar[2].width = screenStep;
		rectChar[2].height = characterHeight;

		rectChar[3].x = screenStep *3;
		rectChar[3].y = topPartHeight;
		rectChar[3].width = screenStep;
		rectChar[3].height = characterHeight;

		rectChar[4].x = screenStep *4;
		rectChar[4].y = topPartHeight;
		rectChar[4].width = screenStep;
		rectChar[4].height = characterHeight;

		talkingRect.x = 0;
		talkingRect.y = topPartHeight + characterHeight;
		talkingRect.width = characterWidth * 5;
		talkingRect.height = talkingHeight;

		dialogueRect.x = 0;
		dialogueRect.y = topPartHeight + characterHeight + talkingHeight;
		dialogueRect.width = dialoguePartWidth;
		dialogueRect.height = dialoguePartHeight;

		frameRect.x = 0;
		frameRect.y = topPartHeight + characterHeight + talkingHeight + dialoguePartHeight;
		frameRect.width = dialoguePartWidth;
		frameRect.height = bottomPartHeight;

		nextRect.x = dialoguePartWidth;
		nextRect.y = topPartHeight + characterHeight + talkingHeight;
		nextRect.width = characterWidth * 5 - dialoguePartWidth;
		nextRect.height = dialoguePartHeight;

		soundRect.x = dialoguePartWidth;
		soundRect.y = topPartHeight + characterHeight + talkingHeight + dialoguePartHeight;
		soundRect.width = characterWidth * 5 - dialoguePartWidth;
		soundRect.height = soundPartHeight;

		rightRect.x = characterWidth * 5;
		rightRect.y = 0;
		rightRect.width = Screen.width - (characterWidth * 5);
		rightRect.height = Screen.height;
	}

	public void DrawBackgrounds() {
		GUI.DrawTexture(headerRect, headerBackground);
		GUI.DrawTexture(rectChar[0], backgroundChars);
		GUI.DrawTexture(rectChar[1], backgroundChars2);
		GUI.DrawTexture(rectChar[2], backgroundChars);
		GUI.DrawTexture(rectChar[3], backgroundChars2);
		GUI.DrawTexture(rectChar[4], backgroundChars);
		GUI.DrawTexture(talkingRect, talkingBackground);
		GUI.DrawTexture(dialogueRect, dialogueBackground);
		GUI.DrawTexture(frameRect, frameBackground);
		GUI.DrawTexture(nextRect, nextBackground);
		GUI.DrawTexture(soundRect, soundBackground);
		GUI.DrawTexture(rightRect, rightBackground);
	}
}