using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DASetText : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueJsonItem data) {

		scene.dialogueText.value = data.text;

		return true;
	}
}
