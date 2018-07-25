using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class DialogueAction : ScriptableObject {

	public abstract bool Act(DialogueScene scene, DialogueActionData data);
	public abstract void FillData(DialogueActionData data);


	public static DialogueAction CreateAction(DActionType type) {
		switch (type)
		{
			case DActionType.START_SCENE: return (DialogueAction)ScriptableObject.CreateInstance("DAStartDialogue");
			case DActionType.END_SCENE: return (DialogueAction)ScriptableObject.CreateInstance("DAEndDialogue");
			case DActionType.SET_TEXT: return (DialogueAction)ScriptableObject.CreateInstance("DASetText");
			case DActionType.SET_MUSIC: return (DialogueAction)ScriptableObject.CreateInstance("DASetBkgMusic");
			case DActionType.SET_CHARS: return (DialogueAction)ScriptableObject.CreateInstance("DASetCharacters");
			case DActionType.SET_BKG: return (DialogueAction)ScriptableObject.CreateInstance("DASetBackground");
			case DActionType.PLAY_SFX: return (DialogueAction)ScriptableObject.CreateInstance("DAPlaySfx");
			case DActionType.MOVEMENT: return (DialogueAction)ScriptableObject.CreateInstance("DAChangePosition");
			case DActionType.FLASH: return (DialogueAction)ScriptableObject.CreateInstance("DAScreenFlash");
			case DActionType.SHAKE: return (DialogueAction)ScriptableObject.CreateInstance("DAScreenShake");
			case DActionType.DELAY: return (DialogueAction)ScriptableObject.CreateInstance("DADelay");
		}

		Debug.LogError("Unsupported type:  " + type);
		return null;
	}
}
