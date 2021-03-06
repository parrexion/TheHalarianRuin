﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuScreenController : MonoBehaviour {

	public enum MenuScreen {STATUS,MODULE,EQUIP,MAP,MESSAGE,OPTIONS,SAVE}

	public IntVariable currentInventoryScreen;
	public BoolVariable menuLock;

	[Header("Screens")]
	public GameObject statusScreen;
	public GameObject moduleScreen;
	public GameObject equipScreen;
	public GameObject mapScreen;
	public GameObject messageScreen;
	public GameObject optionsScreen;
	public GameObject saveScreen;

	[Space(3)]
	[Header("Buttons")]
	public Button statusButton;
	public Button moduleButton;
	public Button equipButton;
	public Button mapButton;
	public Button messageButton;
	public Button optionsButton;
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
#if DEMO_PROLOGUE
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
		if (menuLock.value)
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
		mapScreen.SetActive(screen == MenuScreen.MAP);
		messageScreen.SetActive(screen == MenuScreen.MESSAGE);
		optionsScreen.SetActive(screen == MenuScreen.OPTIONS);
		saveScreen.SetActive(screen == MenuScreen.SAVE);

		//Set current buttons
		statusButton.interactable = (screen != MenuScreen.STATUS);
		moduleButton.interactable = (screen != MenuScreen.MODULE);
		equipButton.interactable = (screen != MenuScreen.EQUIP);
		saveButton.interactable = (screen != MenuScreen.SAVE);
		mapButton.interactable = (screen != MenuScreen.MAP);
		messageButton.interactable = (screen != MenuScreen.MESSAGE);
		optionsButton.interactable = (screen != MenuScreen.OPTIONS);
	}

	/// <summary>
	/// Returns the player to the game again.
	/// </summary>
	public void ReturnToGame() {
		if (menuLock.value)
			return;
		
		menuLock.value = true;
		currentArea.value = playerArea.value;
		buttonClickedEvent.Invoke();
		changeMapEvent.Invoke();
	}

	/// <summary>
	/// Locks the menu until it has faded in.
	/// </summary>
	/// <returns></returns>
	IEnumerator WaitForFadeIn() {
		menuLock.value = true;
		yield return new WaitForSeconds(fadeSpeed.value);
		menuLock.value = false;
		yield break;
	}
}
