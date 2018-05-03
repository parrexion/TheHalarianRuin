using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionsController : MonoBehaviour {

	[Header("Settings")]
	public UnityEvent saveGameEvent;
	public UnityEvent changeMapEvent;

	[Header("Other")]
	public IntVariable currentArea;
	

	/// <summary>
	/// Saves the settings and returns to the main menu.
	/// </summary>
	public void ReturnButtonClick() {
		saveGameEvent.Invoke();
		currentArea.value = (int)Constants.SCENE_INDEXES.MAINMENU;
		changeMapEvent.Invoke();
	}

}
