using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DADelay : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueActionData data) {

		scene.effectStartDuration.value = data.values[0] * 0.001f;
		scene.effectEndDuration.value = 0;

		return true;
	}

    public override void FillData(DialogueActionData data) {
		data.type = DActionType.DELAY;
		data.autoContinue = true;
		data.useDelay = true;
        data.values.Add(250);
    }
}