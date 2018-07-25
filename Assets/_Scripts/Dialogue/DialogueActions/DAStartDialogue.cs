using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DAStartDialogue : DialogueAction {

	public override bool Act (DialogueScene scene, DialogueActionData data)
	{
		scene.background.value = data.entries[0];
		
		MusicEntry me = (MusicEntry)data.entries[1];
		if (me != null) {
			scene.bkgMusic.value = me.clip;
		}
		else {
			Debug.LogWarning("No music set for the scene");
		}

		for (int i = 0; i < Constants.DIALOGUE_PLAYERS_COUNT; i++) {
			scene.characters[i].value = data.entries[i+2];
			scene.poses[i].value = data.values[i];
		}

		return true;
	}

    public override void FillData(DialogueActionData data) {
		data.type = DActionType.START_SCENE;
        data.entries.Add(null);

		data.entries.Add(null);

		for (int i = 0; i < Constants.DIALOGUE_PLAYERS_COUNT; i++) {
			data.entries.Add(null);
			data.values.Add(-1);
		}
    }
}
