﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DAScreenFlash : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueActionData data) {

		scene.effectStartDuration.value = data.values[0] * 0.001f;
		scene.effectEndDuration.value = data.values[1] * 0.001f;

		return true;
	}

    public override void FillData(DialogueActionData data) {
		data.type = DActionType.FLASH;
		data.autoContinue = true;
		data.useDelay = true;
        data.values.Add(50);
        data.values.Add(100);
    }
}