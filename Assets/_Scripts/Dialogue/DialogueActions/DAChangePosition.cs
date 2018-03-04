using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DAChangePosition : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueJsonItem data) {

		ScrObjLibraryEntry temp = scene.characters[data.position1].value;
		scene.characters[data.position1].value = scene.characters[data.position2].value;
		scene.characters[data.position2].value = temp;

		return true;
	}
}
