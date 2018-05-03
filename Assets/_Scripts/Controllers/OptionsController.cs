using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionsController : MonoBehaviour {

	[Header("Values")]
	public IntVariable currentArea;

	[Header("Events")]
	public UnityEvent saveGameEvent;
	public UnityEvent buttonClickedEvent;
	public UnityEvent changeMapEvent;
	

	/// <summary>
	/// Saves the settings and returns to the main menu.
	/// </summary>
	public void ReturnButtonClick() {
		buttonClickedEvent.Invoke();
		saveGameEvent.Invoke();
		currentArea.value = (int)Constants.SCENE_INDEXES.MAINMENU;
		changeMapEvent.Invoke();
	}

}
