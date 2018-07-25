using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAPlaySfx : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueActionData data) {

		SfxEntry sfx = (SfxEntry)data.entries[0];
		if (sfx == null) {
			Debug.LogWarning("Empty Sfx action!");
			return true;
		}

		scene.sfxClip.value = sfx.clip;
		scene.effectStartDuration.value = data.values[0] * 0.001f;
		scene.effectEndDuration.value = 0;

		return true;
	}

    public override void FillData(DialogueActionData data) {
		data.type = DActionType.PLAY_SFX;
		data.autoContinue = true;
		data.useDelay = true;
        data.entries.Add(null);
		data.values.Add(0);
    }
}
