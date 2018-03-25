using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour {

	public enum MenuScreen {STATUS,MODULE,EQUIP,MAP,MESSAGE,JOURNAL,SAVE}

	public MenuScreen currentScreen = MenuScreen.STATUS;
	public bool isEditor = false;
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
	public IntVariable playerArea;
	public FloatVariable fadeSpeed;

	public UnityEvent buttonClickedEvent;
	public UnityEvent changeMapEvent;


	// Use this for initialization
	void Start () {
		StartCoroutine(WaitForFadeIn());
#if UNITY_EDITOR
		isEditor = true;
#else
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
		MenuScreen screen = (MenuScreen)screenIndex;
		if (currentScreen != screen) {
			currentScreen = screen;
			UpdateCurrentScreen();
		}
	}

	void UpdateCurrentScreen() {

		//Set current screen
		statusScreen.SetActive(currentScreen == MenuScreen.STATUS);
		moduleScreen.SetActive(currentScreen == MenuScreen.MODULE);
		equipScreen.SetActive(currentScreen == MenuScreen.EQUIP);
		if (isEditor) {
			mapScreen.SetActive(currentScreen == MenuScreen.MAP);
			messageScreen.SetActive(currentScreen == MenuScreen.MESSAGE);
			journalScreen.SetActive(currentScreen == MenuScreen.JOURNAL);
			saveScreen.SetActive(currentScreen == MenuScreen.SAVE);
		}

		//Set current buttons
		statusButton.interactable = (currentScreen != MenuScreen.STATUS);
		moduleButton.interactable = (currentScreen != MenuScreen.MODULE);
		equipButton.interactable = (currentScreen != MenuScreen.EQUIP);
		if (isEditor) {
			mapButton.interactable = (currentScreen != MenuScreen.MAP);
			messageButton.interactable = (currentScreen != MenuScreen.MESSAGE);
			journalButton.interactable = (currentScreen != MenuScreen.JOURNAL);
			saveButton.interactable = (currentScreen != MenuScreen.SAVE);
		}
	}

	/// <summary>
	/// Returns the player to the game again.
	/// </summary>
	public void ReturnToGame() {
		if (menuLock)
			return;
		
		menuLock = true;
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
