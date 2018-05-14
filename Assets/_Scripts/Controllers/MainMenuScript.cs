using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour {

	public Canvas mainMenuCanvas;
	public Canvas loadGameCanvas;

	[Header("Areas")]
	public IntVariable currentChapter;
	public IntVariable currentArea;
	public IntVariable currentRoomNumber;
	public IntVariable playerArea;

	[Header("Story stuff")]
	public DialogueEntry startDialogue;
	public Constants.CHAPTER startChapter;
	public StringVariable dialogueUuid;
	public BoolVariable playingAsAndroid;
	public BoolVariable useFollower;
	
	[Header("Events")]
	public UnityEvent buttonClickEvent;
	public UnityEvent mapChangeEvent;
	public UnityEvent dialogueEvent;
	public UnityEvent startNewgameEvent;


	void Start() {
		startNewgameEvent.Invoke();
	}

	/// <summary>
	/// Options clicked
	/// </summary>
	public void OptionsClicked() {
		buttonClickEvent.Invoke();
		currentArea.value = (int)Constants.SCENE_INDEXES.OPTIONS;
		mapChangeEvent.Invoke();
	}

	/// <summary>
	/// Return to the main menu canvas.
	/// </summary>
	public void ReturnToMainCanvas() {
		buttonClickEvent.Invoke();
		mainMenuCanvas.enabled = true;
		loadGameCanvas.enabled = false;
	}

	/// <summary>
	/// New game selected.
	/// </summary>
	public void NewgameClicked() {
		buttonClickEvent.Invoke();
		
		dialogueUuid.value = startDialogue.uuid;
		currentChapter.value = (int)startChapter;
		currentRoomNumber.value = 0;

		dialogueEvent.Invoke();
	}

	/// <summary>
	/// Load game selected.
	/// </summary>
	public void LoadGameClicked() {
		buttonClickEvent.Invoke();
		mainMenuCanvas.enabled = false;
		loadGameCanvas.enabled = true;
	}

}
