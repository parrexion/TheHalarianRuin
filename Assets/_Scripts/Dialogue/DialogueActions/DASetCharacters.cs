using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DASetCharacters : DialogueAction {

	public override bool Act (DialogueScene scene, DialogueActionData data)
	{
		for (int i = 0; i < Constants.DIALOGUE_PLAYERS_COUNT; i++) {
			scene.characters[i].value = data.entries[i];
			scene.poses[i].value = data.values[i];
		}

		scene.effectStartDuration.value = 0;
		scene.effectEndDuration.value = 0;

		return true;
	}

    public override void FillData(DialogueActionData data) {
		data.type = DActionType.SET_CHARS;
		data.autoContinue = true;
		data.useDelay = false;
		for (int i = 0; i < Constants.DIALOGUE_PLAYERS_COUNT; i++) {
			data.entries.Add(null);
			data.values.Add(-1);
		}
    }
}
