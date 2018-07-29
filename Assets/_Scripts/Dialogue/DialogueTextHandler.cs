using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DialogueTextHandler : MonoBehaviour {

	public StringVariable dialogueText;
	public StringVariable inputText;
	public string currentDialogue;
	public string finishedDialogue;
	public StringVariable shownDialogueText;
	public Text dialogueTextBox;
	public CursorBobber cursorBobber;
	public FloatVariable blipSfxDelay;
	// public GameObject choiceBox;
	public UnityEvent nextFrameEvent;
	public UnityEvent updateDialogueTextEvent;
	public UnityEvent playDialogueBlipEvent;

	private string[] words = new string[0];
	private bool textUpdating = false;
	private bool stopText = false;


	// Use this for initialization
	void Start () {
		dialogueText.value = "";
		inputText.value = "";
	}

	void Update() {
		if (textUpdating) {
			shownDialogueText.value = dialogueText.value + currentDialogue;
			updateDialogueTextEvent.Invoke();
		}
	}
	
	/// <summary>
	/// Updates the dialogue box when it is clicked.
	/// </summary>
	public void DialogueBoxClicked() {

		if (textUpdating) {
			// Finish the text of this frame
			cursorBobber.ShowCursor(true);
			StopAllCoroutines();
			textUpdating = false;
			dialogueText.value += finishedDialogue;
			currentDialogue = finishedDialogue;
		}
		else {
			// Move on to the next frame
			cursorBobber.ShowCursor(false);
			nextFrameEvent.Invoke();
		}

		shownDialogueText.value = dialogueText.value;
		updateDialogueTextEvent.Invoke();
	}

	/// <summary>
	/// Parses the current dialogue line and splits it into words and sets the characters.
	/// </summary>
	public void ParseLine() {
		cursorBobber.ShowCursor(false);
		StopAllCoroutines();
		currentDialogue = "";
		stopText = false;

		words = inputText.value.Split(' ');
		bool first = true;
		finishedDialogue = "";
		for (int i = 0; i < words.Length; i++) {
			if (string.IsNullOrEmpty(words[i]))
				continue;
			if (!IsSpecialWord(words[i])) {
				if (first) {
					finishedDialogue += words[i];
					first = false;
				}
				else
					finishedDialogue += " " + words[i];
			}
		}

		StartCoroutine(TextUpdate());
	}

	/// <summary>
	/// Updates the current dialogue line, character by character
	/// </summary>
	/// <returns></returns>
	IEnumerator TextUpdate() {
		float timeInSeconds = .025f;
		float currentDelay = 0;
		textUpdating = true;

		for (int j = 0; j < words.Length; j++) {
			if (stopText)
				break;
			if (words[j].Length == 0)
				continue;
			if (IsSpecialWord(words[j]))
				continue;
			if (FitNextWord(words[j]+" "))
				currentDialogue += "\n";
			for (int i = 0; i < words[j].Length; i++) {
				currentDialogue += words[j][i];
				currentDelay += timeInSeconds;
				if (currentDelay >= blipSfxDelay.value) {
					currentDelay -= blipSfxDelay.value;
					playDialogueBlipEvent.Invoke();
				}
				yield return new WaitForSeconds(timeInSeconds);
			}
			currentDialogue += ' ';

			yield return new WaitForSeconds(timeInSeconds);
		}
		if (stopText) {
			DialogueBoxClicked();
		}
		DialogueBoxClicked();

	}

	/// <summary>
	/// Calculates whether the next word will need a newline. True if a newline is required.
	/// </summary>
	/// <param name="nextWord"></param>
	/// <returns></returns>
	private bool FitNextWord(string nextWord){
		string word = nextWord.Split('\n')[0];
		TextGenerationSettings settings = dialogueTextBox.GetGenerationSettings(dialogueTextBox.rectTransform.rect.size);
		float originalHeight = dialogueTextBox.cachedTextGeneratorForLayout.GetPreferredHeight(dialogueText.value + currentDialogue,settings);
		float newHeight = dialogueTextBox.cachedTextGeneratorForLayout.GetPreferredHeight(dialogueText.value + currentDialogue+word,settings);

		return newHeight > originalHeight;
	}

	bool IsSpecialWord(string word) {
		if (word[0] == '£') {
			word = "";
			stopText = true;
			return true;
		}
// 		else if (word[0] == '@') {
// 			Debug.Log("Found an sfx!");
// 			string sfxString = word.Substring(1);
// 			Debug.Log(sfxString);
// 			SfxEntry sfx = (SfxEntry)sfxLibrary.GetEntry(sfxString);
// #if !UNITY_EDITOR
// 			if (sfx == null)
// 				return true;
// #endif
// 			sfxClip.value = sfx.clip;
// 			if (sfxClip.value != null)
// 				playSfx.Invoke();
// 			else
// 				Debug.LogWarning("SFX clip is null in the dialogue!");
// 			return true;
// 		}
// 		else if (word[0] == '¤') {
// 			Debug.Log("Found a screen flash!");
// 			screenFlashEvent.Invoke();
// 			return true;
// 		}
// 		else if (word[0] == '§') {
// 			Debug.Log("Found a screen shake!");
// 			string shakeString = word.Substring(1);
// 			shakeDuration.value = float.Parse(shakeString);
// 			screenShakeEvent.Invoke();
// 			return true;
// 		}

		return false;
	}




	// /// <summary>
	// /// Creates choice buttons in the dialogue
	// /// </summary>
	// void CreateButtons() {
	// 	for(int i = 0 ; i < options.Length; i++) {
	// 		GameObject button = (GameObject)Instantiate(choiceBox);
	// 		Button b = button.GetComponent<Button>();
	// 		ChoiceButton cb = button.GetComponent<ChoiceButton>();
	// 		cb.SetText(options[i].Split(':')[0]);
	// 		cb.option = options[i].Split(':')[1];
	// 		cb.box = this;
	// 		b.transform.SetParent(this.transform);
	// 		b.transform.localPosition = new Vector3(0,-25+(i*50));
	// 		b.transform.localScale = new Vector3(1,1,1);
	// 		buttons.Add(b);
	// 	}
	// }

	// /// <summary>
	// /// Removes the dialogue buttons.
	// /// </summary>
	// void ClearButtons() {
	// 	for (int i = 0; i < buttons.Count; i++) {
	// 		Button b = buttons[i];
	// 		buttons.Remove(b);
	// 		Destroy(b.gameObject);
	// 	}
	// }
}
