using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DASetBkgMusic : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueJsonItem data) {

		MusicEntry me = (MusicEntry)data.entry;
		scene.bkgMusic.value = me.clip;

		return true;
	}
}
