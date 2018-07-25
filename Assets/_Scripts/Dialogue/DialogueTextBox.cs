using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTextBox : MonoBehaviour {

	public StringVariable dialogueText;
	public Text textBox;


	private void Start() {
		textBox.text = "";
	}

	public void UpdateText() {
		textBox.text = dialogueText.value;
	}
}
