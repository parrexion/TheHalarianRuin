using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MainMenuScript : MonoBehaviour {

	public Canvas mainMenuCanvas;
	public Canvas loadGameCanvas;
	public Canvas levelSelectCanvas;

	[Header("Areas")]
	public StringVariable currentChapter;
	public IntVariable currentArea;
	public IntVariable currentRoomNumber;
	public IntVariable playerArea;

	[Header("Story stuff")]
	public string dialogueUuidStr = "WakeUpAndroid";
	public string currentChapterStr = "Prologue";
	public StringVariable dialogueUuid;
	public BoolVariable playingAsAndroid;
	public BoolVariable useFollower;
	
	[Header("Events")]
	public UnityEvent buttonClickEvent;
	public UnityEvent mapChangeEvent;
	public UnityEvent dialogueEvent;
	public UnityEvent startNewgameEvent;

	[Header("Battle tower stuff")]
	public Text recordText;
	public IntVariable bestTowerLevel;
	public IntVariable currentTowerLevel;
	public Button levelMaxButton;
	public Text levelMaxText;
	public Button levelMinus5Button;
	public Text levelMinus5Text;


	void Start() {
		recordText.text = "Highest level: " + bestTowerLevel.value;
		currentArea.value = (int)Constants.SCENE_INDEXES.MAINMENU;
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
		mainMenuCanvas.enabled = true;
		loadGameCanvas.enabled = false;
		levelSelectCanvas.enabled = false;
	}

	/// <summary>
	/// New game selected.
	/// </summary>
	public void NewgameClicked() {
		buttonClickEvent.Invoke();
		
		dialogueUuid.value = dialogueUuidStr;
		currentRoomNumber.value = 0;

		startNewgameEvent.Invoke();
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

	/// <summary>
	/// Random battles clicked
	/// </summary>
	public void BattleClicked() {
		mainMenuCanvas.enabled = false;
		levelSelectCanvas.enabled = true;
		int bestLevel = bestTowerLevel.value;

		levelMaxButton.GetComponentInChildren<Text>().text = "LEVEL " + bestLevel;
		levelMinus5Button.GetComponentInChildren<Text>().text = "LEVEL " + (bestLevel - 5);

		levelMaxButton.gameObject.SetActive(bestLevel > 1);
		levelMaxText.gameObject.SetActive(bestLevel > 1);
		levelMinus5Button.gameObject.SetActive(bestLevel > 6);
		levelMinus5Text.gameObject.SetActive(bestLevel > 6);
	}

	/// <summary>
	/// Battle tower clicked
	/// </summary>
	/// <param name="levelPosition"></param>
	public void LevelSelectClicked(int levelPosition) {
		int bestLevel = bestTowerLevel.value;
		if (levelPosition == 1) 
			currentTowerLevel.value = 1;
		else if (levelPosition == 3)
			currentTowerLevel.value = bestLevel;
		else
			currentTowerLevel.value = bestLevel - 5;

		buttonClickEvent.Invoke();
		currentArea.value = (int)Constants.SCENE_INDEXES.BATTLETOWER;
		playerArea.value = (int)Constants.SCENE_INDEXES.BATTLETOWER;
		mapChangeEvent.Invoke();
	}

}
