using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DASetName : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueJsonItem data) {

		scene.talkingName.value = data.text;

		return true;
	}
}
