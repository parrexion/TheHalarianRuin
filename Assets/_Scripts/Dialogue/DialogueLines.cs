using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[RequireComponent(typeof(DialogueScene))]
public class DialogueLines : MonoBehaviour {

	public ScrObjLibraryVariable dialogueLibrary;
	public StringVariable dialogueUuid;
	private DialogueEntry dialogueEntry;

	public IntVariable currentAction;
	public BoolVariable overrideActionNumber;
	private DialogueScene scene;

	private bool isWaiting;


	void Start() {
		scene = GetComponent<DialogueScene>();
		dialogueLibrary.initialized = false;
		dialogueLibrary.GenerateDictionary();
		dialogueEntry = (DialogueEntry)dialogueLibrary.GetEntry(dialogueUuid.value);
		if (!overrideActionNumber.value) {
			currentAction.value = 0;
			scene.Reset();
		}
		else {
			overrideActionNumber.value = false;
		}
		NextFrame();

		scene.backgroundChanged.Invoke();
		scene.bkgMusicChanged.Invoke();
		scene.characterChanged.Invoke();
		scene.closeupChanged.Invoke();
		scene.dialogueTextChanged.Invoke();
	}

	public void NextFrame(){

		if (isWaiting)
			return;

		StartCoroutine(RunNextFrame());
	}

	private IEnumerator RunNextFrame() {
		isWaiting = true;
		while(currentAction.value < dialogueEntry.actions.Count) {
			DialogueActionData data = dialogueEntry.actions[currentAction.value];
			DialogueAction da = DialogueAction.CreateAction(data.type);
			da.Act(scene, data);
			if (data.type == DActionType.MOVEMENT) {
				for (int i = 0; i < Constants.DIALOGUE_PLAYERS_COUNT+2; i++) {
					float speed = data.values[0] * 0.001f;
					scene.characterTransforms[i].MoveCharacter(speed);
				}
			}

			RunEvents(data.type);
			currentAction.value++;

			if (data.useDelay) {
				if (data.type != DActionType.SET_TEXT && data.type != DActionType.END_SCENE) {
					yield return new WaitForSeconds(scene.effectStartDuration.value);
					yield return new WaitForSeconds(scene.effectEndDuration.value);
				}
			}

			if (!data.autoContinue)
				break;
		}
		isWaiting = false;
		yield break;
	}

	private bool CheckContinueAction(DialogueActionData data) {
		if (data.type == DActionType.SET_TEXT)
			return false;

		return true;
	}

	private void RunEvents(DActionType type) {
		switch (type)
		{
			case DActionType.START_SCENE:
				scene.backgroundChanged.Invoke();
				scene.bkgMusicChanged.Invoke();
				scene.characterChanged.Invoke();
				break;
			case DActionType.END_SCENE: break;
			case DActionType.SET_TEXT: 
				scene.closeupChanged.Invoke();
				scene.dialogueTextChanged.Invoke();
				break;
			case DActionType.SET_CHARS: scene.characterChanged.Invoke(); break;
			case DActionType.SET_BKG: scene.backgroundChanged.Invoke(); break;
			case DActionType.SET_MUSIC: scene.bkgMusicChanged.Invoke();break;
			case DActionType.PLAY_SFX: scene.playSfx.Invoke(); break;
			case DActionType.MOVEMENT: scene.characterChanged.Invoke(); break;
			case DActionType.FLASH: scene.screenFlashEvent.Invoke(); break;
			case DActionType.SHAKE: scene.screenShakeEvent.Invoke(); break;
		}	
	}

}
