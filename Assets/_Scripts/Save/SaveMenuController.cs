using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class which handles the system menu in the inventory screen and toggles the screens.
/// </summary>
public class SaveMenuController : MonoBehaviour {

	public SaveFileCurrentProgress progress;
	public BoolVariable isCurrentlySaving;

	[Header("Events")]
	public AreaIntVariable currentArea;
	public UnityEvent changeMapEvent;

	[Header("Screens")]
	public GameObject buttonScreen;
	public GameObject saveLoadScreen;
	public GameObject quitPopup;


	private void OnEnable() {
		ShowButtonScreen();
	}

	/// <summary>
	/// Shows the menu buttons screen.
	/// </summary>
	public void ShowButtonScreen() {
		buttonScreen.SetActive(true);
		saveLoadScreen.SetActive(false);
		quitPopup.SetActive(false);
	}

	/// <summary>
	/// Shows the save and load screen.
	/// </summary>
	public void ShowSaveLoadScreen(bool isSaving) {
		isCurrentlySaving.value = isSaving;
		buttonScreen.SetActive(false);
		saveLoadScreen.SetActive(true);
		quitPopup.SetActive(false);
	}

	/// <summary>
	/// Shows the quit without saving popup.
	/// </summary>
	public void ShowQuitPopup() {
		buttonScreen.SetActive(false);
		saveLoadScreen.SetActive(false);
		quitPopup.SetActive(true);
	}

	/// <summary>
	/// Quits the current game and returns to the main menu.
	/// </summary>
	public void QuitToMain() {
		currentArea.value = (int)(Constants.SCENE_INDEXES.MAINMENU);
		changeMapEvent.Invoke();
	}
}
