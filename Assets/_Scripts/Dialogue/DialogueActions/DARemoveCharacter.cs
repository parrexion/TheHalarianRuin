using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DARemoveCharacter : DialogueAction {

	public override bool Act (DialogueScene scene, DialogueJsonItem data)
	{
		scene.characters[data.position1] = null;
		return true;
	}
}
