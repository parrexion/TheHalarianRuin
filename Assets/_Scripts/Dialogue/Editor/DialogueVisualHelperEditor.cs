using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogueVisualContainer))]
public class DialogueVisualHelperEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector();

		if (GUILayout.Button("Update Scene")){
			DialogueVisualContainer dvc = target as DialogueVisualContainer;
			SetScene(dvc);
		}
	}

	public void SetScene(DialogueVisualContainer dvc) {
		dvc.background.UpdateBackground();
		for (int i = 0; i < dvc.characters.Length; i++) {
			dvc.characters[i].UpdateCharacter();
		}
		dvc.closeup.UpdateCloseup();
		dvc.textBox.text = dvc.currentDialogueText.value;
		dvc.musicText.text = (dvc.currentMusic.value) ? dvc.currentMusic.value.name : "[NO MUSIC]";
	}
}
