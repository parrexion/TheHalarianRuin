using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DAEndDialogue : DialogueAction {

	// public override bool Act(DialogueScene scene, DialogueActionData data) {
	// 	DialogueEntry de = (DialogueEntry)data.entries[0];
	// 	switch (de.nextLocation)
	// 	{
	// 		case BattleEntry.NextLocation.OVERWORLD:
	// 			// scene.paused.value = false;
	// 			if (de.changePosition) {
	// 				if (de.nextArea != Constants.OverworldArea.DEFAULT)
	// 					scene.playerArea.value = (int)de.nextArea;
	// 				scene.playerPosX.value = de.playerPosition.x;
	// 				scene.playerPosY.value = de.playerPosition.y;
	// 			}
	// 			scene.currentArea.value = scene.playerArea.value;
	// 			Debug.Log("Moving to area  " + scene.currentArea.value);
	// 			scene.mapChangeEvent.Invoke();
	// 			break;
	// 		case BattleEntry.NextLocation.DIALOGUE:
	// 			scene.currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
	// 			scene.dialogueUuid.value = de.dialogueEntry.uuid;
	// 			scene.mapChangeEvent.Invoke();
	// 			break;
	// 		case BattleEntry.NextLocation.BATTLE:
	// 			scene.currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
	// 			scene.battleUuid.value = de.battleEntry.uuid;
	// 			scene.mapChangeEvent.Invoke();
	// 			break;
	// 	}

	// 	return false;
	// }

	//Entry 0	BattleEntry
	//Entry 1	DialogueEntry

	/*  Values
	0	NextLocation
	1	Changed pos - bool
	2	Next area index
	3	Player xpos
	4	Player ypos
	*/

	public override bool Act(DialogueScene scene, DialogueActionData data) {
		BattleEntry.NextLocation next = (BattleEntry.NextLocation)data.values[0];
		switch (next)
		{
			case BattleEntry.NextLocation.OVERWORLD:
				// scene.paused.value = false;
				if (data.values[1] == 1) { //Changed pos
					if ((Constants.OverworldArea)data.values[2] != Constants.OverworldArea.DEFAULT)
						scene.playerArea.value = data.values[2];
					scene.playerPosX.value = data.values[3];
					scene.playerPosY.value = data.values[4];
				}
				scene.currentArea.value = scene.playerArea.value;
				Debug.Log("Moving to area  " + scene.currentArea.value);
				scene.mapChangeEvent.Invoke();
				break;
			case BattleEntry.NextLocation.DIALOGUE:
				scene.currentArea.value = (int)Constants.SCENE_INDEXES.DIALOGUE;
				scene.dialogueUuid.value = data.entries[0].uuid;
				scene.mapChangeEvent.Invoke();
				break;
			case BattleEntry.NextLocation.BATTLE:
				scene.currentArea.value = (int)Constants.SCENE_INDEXES.BATTLE;
				scene.battleUuid.value = data.entries[1].uuid;
				scene.mapChangeEvent.Invoke();
				break;
		}

		return false;
	}

    public override void FillData(DialogueActionData data) {
		data.type = DActionType.END_SCENE;
		data.autoContinue = false;
		data.useDelay = false;
        data.entries.Add(null);
        data.entries.Add(null);

        data.values.Add(0);
        data.values.Add(0);
        data.values.Add(0);
        data.values.Add(0);
        data.values.Add(0);
    }
}
