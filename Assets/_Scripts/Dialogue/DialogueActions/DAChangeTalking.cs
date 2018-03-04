using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DAChangeTalking : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueJsonItem data) {

		scene.talkingChar.value = data.entry;
		scene.talkingPose.value = data.value;

		return true;
	}
}
