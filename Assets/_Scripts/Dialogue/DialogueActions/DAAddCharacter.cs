using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DAAddCharacter : DialogueAction {

	public override bool Act (DialogueScene scene, DialogueJsonItem data)
	{
		scene.characters[data.position1].value = data.entry;
		scene.poses[data.position1].value = data.value;

		return true;
	}
}
