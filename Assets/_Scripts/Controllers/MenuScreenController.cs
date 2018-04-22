using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour {

	public enum MenuScreen {STATUS,MODULE,EQUIP,MAP,MESSAGE,JOURNAL,SAVE}

	public IntVariable currentInventoryScreen;
	bool isEditor = false;
	bool menuLock = true;

	[Header("Screens")]
	public GameObject statusScreen;
	public GameObject moduleScreen;
	public GameObject equipScreen;
	public GameObject mapScreen;
	public GameObject messageScreen;
	public GameObject journalScreen;
	public GameObject saveScreen;

	[Space(3)]
	[Header("Buttons")]
	public Button statusButton;
	public Button moduleButton;
	public Button equipButton;
	public Button mapButton;
	public Button messageButton;
	public Button journalButton;
	public Button saveButton;

	[Header("Other values")]
	public AreaIntVariable playerArea;
	public AreaIntVariable currentArea;
	public FloatVariable fadeSpeed;

	public UnityEvent buttonClickedEvent;
	public UnityEvent changeMapEvent;


	// Use this for initialization
	void Start () {
		StartCoroutine(WaitForFadeIn());
#if UNITY_EDITOR
		isEditor = true;
#else
		isEditor = false;
		statusButton.gameObject.SetActive(false);
		moduleButton.gameObject.SetActive(false);
		equipButton.gameObject.SetActive(false);
		mapButton.gameObject.SetActive(false);
		messageButton.gameObject.SetActive(false);
		journalButton.gameObject.SetActive(false);
		saveButton.gameObject.SetActive(false);
#endif
		UpdateCurrentScreen();
	}

	/// <summary>
	/// Sets which inventory screen to show.
	/// </summary>
	/// <param name="screenIndex"></param>
	public void SetCurrentScreen(int screenIndex) {
		if (menuLock)
			return;

		buttonClickedEvent.Invoke();
		if (currentInventoryScreen.value != screenIndex) {
			currentInventoryScreen.value = screenIndex;
			UpdateCurrentScreen();
		}
	}

	/// <summary>
	/// Enables the currently selected screen, hiding the ones which aren't used atm.
	/// </summary>
	void UpdateCurrentScreen() {
		MenuScreen screen = (MenuScreen)currentInventoryScreen.value;
		//Set current screen
		statusScreen.SetActive(screen == MenuScreen.STATUS);
		moduleScreen.SetActive(screen == MenuScreen.MODULE);
		equipScreen.SetActive(screen == MenuScreen.EQUIP);
		if (isEditor) {
			mapScreen.SetActive(screen == MenuScreen.MAP);
			messageScreen.SetActive(screen == MenuScreen.MESSAGE);
			journalScreen.SetActive(screen == MenuScreen.JOURNAL);
			saveScreen.SetActive(screen == MenuScreen.SAVE);
		}

		//Set current buttons
		statusButton.interactable = (screen != MenuScreen.STATUS);
		moduleButton.interactable = (screen != MenuScreen.MODULE);
		equipButton.interactable = (screen != MenuScreen.EQUIP);
		if (isEditor) {
			mapButton.interactable = (screen != MenuScreen.MAP);
			messageButton.interactable = (screen != MenuScreen.MESSAGE);
			journalButton.interactable = (screen != MenuScreen.JOURNAL);
			saveButton.interactable = (screen != MenuScreen.SAVE);
		}
	}

	/// <summary>
	/// Returns the player to the game again.
	/// </summary>
	public void ReturnToGame() {
		if (menuLock)
			return;
		
		menuLock = true;
		currentArea.value = playerArea.value;
		buttonClickedEvent.Invoke();
		changeMapEvent.Invoke();
	}

	/// <summary>
	/// Locks the menu until it has faded in.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForFadeIn() {
		yield return new WaitForSeconds(fadeSpeed.value);
		menuLock = false;
		yield break;
	}
}
