using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DAEndDialogue : DialogueAction {

	public override bool Act(DialogueScene scene, DialogueJsonItem data) {
		DialogueEntry de = (DialogueEntry)data.entry;
		switch (de.nextLocation)
		{
			case BattleEntry.NextLocation.OVERWORLD:
				scene.paused.value = false;
				if (de.changePosition) {
					if (de.nextArea != Constants.OverworldArea.DEFAULT)
						scene.playerArea.value = (int)de.nextArea;
					scene.playerPosX.value = de.playerPosition.x;
					scene.playerPosY.value = de.playerPosition.y;
				}
				scene.currentArea.value = scene.playerArea.value;
				scene.mapChangeEvent.Invoke();
				break;
			case BattleEntry.NextLocation.DIALOGUE:
				scene.currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
				scene.dialogueUuid.value = de.nextEntry.uuid;
				scene.mapChangeEvent.Invoke();
				break;
			case BattleEntry.NextLocation.BATTLE:
				scene.currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
				scene.battleUuid.value = de.nextEntry.uuid;
				scene.mapChangeEvent.Invoke();
				break;
		}

		return false;
	}
}
