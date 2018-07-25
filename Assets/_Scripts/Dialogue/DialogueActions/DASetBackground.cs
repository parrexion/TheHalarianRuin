using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DASetBackground : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueActionData data) {

		scene.background.value = data.entries[0];

		scene.effectStartDuration.value = 0;
		scene.effectEndDuration.value = 0;

		return true;
	}

    public override void FillData(DialogueActionData data) {
		data.type = DActionType.SET_BKG;
		data.autoContinue = true;
		data.useDelay = false;
        data.entries.Add(null);
    }
}
